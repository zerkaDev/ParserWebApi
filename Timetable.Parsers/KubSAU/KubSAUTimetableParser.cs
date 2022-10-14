using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Domain;

namespace Timetable.Parsers.KubSAU
{
    public class KubSAUTimetableParser : ITimetableParser
    {
        internal class Root
        {
            public string query { get; set; }
            public List<Suggestion> suggestions { get; set; }
        }

        internal class Suggestion
        {
            public string value { get; set; }
            public string data { get; set; }
        }
        public string UniversityName { get; } = "KubSAU";
        string BaseUrl { get; } = "https://s.kubsau.ru/education/schedule/";
        UriBuilder UriBuilder { get; }
        HtmlWeb HtmlWeb { get; }
        HtmlDocument LastRequestDocument { get; set; }
        IEnumerable<string> groupNames { get; set; }
        ParserHelper ParserHelper { get; set; }
        Dictionary<string, string> NumberLessonsDict = new Dictionary<string, string>()
        {
            {
                "08:00","1 пара"
            },
            {
                "09:45","2 пара"
            },
            {
                "11:30","3 пара"
            },
            {
                "13:50","4 пара"
            },
            {
                "15:35","5 пара"
            },
            {
                "17:20","6 пара"
            }
        };
        public KubSAUTimetableParser()
        {
            UriBuilder = new(BaseUrl);
            HtmlWeb = new();
            groupNames = new List<string>();
            ParserHelper = new();
            FillGroupNames();

        }
        private void FillGroupNames()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://s.kubsau.ru/bitrix/components/atom/atom.education.schedule-real/get.php?query=%&type_schedule=1");

            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
                groupNames = myDeserializedClass.suggestions.Select(x => x.value);
            }
        }
        public Task<List<Lesson>> GetLessonsOfThisDay(bool IsOddWeek, int dayNumber)
        {
            var lessonsVm = new List<Lesson>();

            string xpath = "";

            if (IsOddWeek)
                xpath = $"//div[@class='card card-sched schedule-first-week']";
            else
                xpath = $"//div[@class='card card-sched schedule-second-week']";

            var lessons = LastRequestDocument.DocumentNode
                    .SelectSingleNode(xpath)
                    .ChildNodes.Where(node => node.Name == "div")
                        .ElementAt(dayNumber - 1)
                        .ChildNodes[3]
                        .ChildNodes[1]
                        .ChildNodes
                        .Where(n => n.Name == "tr");

            foreach (var lesson in lessons)
            {
                var lessonName = lesson.ChildNodes[5].ChildNodes[0].InnerText.Trim();
                if (string.IsNullOrWhiteSpace(lessonName)) continue;
                var duration = lesson.ChildNodes[1].InnerHtml.Replace("<br>", ":");
                var typeLesson = lesson.ChildNodes[3].Attributes[0].Value.Contains("yes") ? "Лекция" : "Практика";

                // Can return 2 teachers and string.Empty
                var teacherShortName = lesson.ChildNodes[5].ChildNodes.FirstOrDefault(node => node.Name == "span").InnerText.Trim();

                var audience = lesson.ChildNodes[7].ChildNodes[1].InnerHtml.Contains("<br>") ?
                    lesson.ChildNodes[7].ChildNodes[1].InnerHtml.Replace("<br>", ", ")
                    :
                    lesson.ChildNodes[7].ChildNodes[1].InnerText;

                var numberLesson = NumberLessonsDict[duration.Substring(0, 5)];

                if (string.IsNullOrWhiteSpace(teacherShortName))
                    lessonsVm.Add(ParserHelper.CreateLesson(audience, duration, lessonName, numberLesson, typeLesson, null));
                else if (teacherShortName.Contains(','))
                {
                    var teachers = teacherShortName.Split(',');
                    foreach (var teacherSplittedName in teachers)
                    {
                        var teacher = ParserHelper.GetTeacherOrDefault(teacherSplittedName);
                        var lessonDto = ParserHelper.CreateLesson(audience, duration, lessonName, numberLesson, typeLesson, teacher);
                        lessonsVm.Add(lessonDto);
                    }
                }
                else
                {
                    var teacher = ParserHelper.GetTeacherOrDefault(teacherShortName);
                    var lessonDto = ParserHelper.CreateLesson(audience, duration, lessonName, numberLesson, typeLesson, teacher);
                    lessonsVm.Add(lessonDto);
                }
            }
            return Task.FromResult(lessonsVm);
        }
        public Task<List<OneDayTimetable>> GetAllDaysOfWeek(bool IsOddWeek)
        {
            IEnumerable<HtmlNode> daysOnWeek;
            List<OneDayTimetable> days = new List<OneDayTimetable>();

            if (IsOddWeek)
                daysOnWeek = LastRequestDocument.DocumentNode
                    .SelectSingleNode($"//div[@class='card card-sched schedule-first-week']")
                        .ChildNodes.Where(node => node.Name == "div");
            else
                daysOnWeek = LastRequestDocument.DocumentNode
                    .SelectSingleNode($"//div[@class='card card-sched schedule-second-week']")
                        .ChildNodes.Where(node => node.Name == "div");


            // Это для того чтобы не заполнять бд днями без пар, но изза этого страдает производительность
            // так как по факту буду проходить по парам еще в другом методе
            foreach (var day in daysOnWeek)
            {
                var lessonsOfThisDay = day.ChildNodes[3].ChildNodes[1].ChildNodes.Where(node => node.Name == "tr");
                bool IsDayEmpty = true;
                foreach (var lesson in lessonsOfThisDay)
                {
                    if (!string.IsNullOrWhiteSpace(lesson.ChildNodes[5].ChildNodes[0].InnerText))
                    {
                        IsDayEmpty = false;
                        break;
                    }
                }
                if (!IsDayEmpty) days.Add(ParserHelper.CreateDay(day.ChildNodes[1].InnerText.Substring(0, day.ChildNodes[1].InnerText.IndexOf('|') - 1)));
            }
            return Task.FromResult(days);
        }
        public Task<List<Week>> GetAllWeeksOfGroup(int fak_id, int kurs, string groupName)
        {
            //// Вроде всегда 2 недели, возвращаем две недели
            UriBuilder.Query = $"?type_schedule=1&val={groupName}";
            var doc = HtmlWeb.Load(UriBuilder.Uri);
            LastRequestDocument = doc;

            ////
            //var weeks = doc.DocumentNode.SelectSingleNode($"//div[@class='row sched ned']").ChildNodes.Where(node => node.Name == "div");

            return Task.FromResult(
                new List<Week>()
                {
                    ParserHelper.CreateWeek(false),
                    ParserHelper.CreateWeek(true)
                });
        }
        public Task<List<Group>> GetAllGroupsOfCourse(int fak_id, int kurs)
        {
            var groups = new List<Group>();
            foreach (var name in groupNames)
            {
                groups.Add(new Group()
                {
                    Name = name
                });
            }
            return Task.FromResult(groups);
        }

        public Task<List<Course>> GetAllCoursesOfInstitute()
        {
            return Task.FromResult(new List<Course>() { ParserHelper.CreateCourse(0) });
        }
        // TODO: Id !!!!
        public Task<List<Institute>> GetAllInstitutes()
        {
            return Task.FromResult(new List<Institute>() { new Institute() { Name = "StandartInstitute", Id = 9999 } });
        }
    }
}
