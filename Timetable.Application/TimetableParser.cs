using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timetable.Domain;

namespace Timetable.Application
{
    /// <summary>
    /// Parser from HTML to POCO. [KUBSTU onyly]!
    /// </summary>
    public class TimetableParser
    {
        string BaseUrl { get; } = "https://elkaf.kubstu.ru/timetable/default/time-table-student-ofo";
        UriBuilder UriBuilder { get; }
        HtmlWeb HtmlWeb { get; }

        public TimetableParser()
        {
            UriBuilder uriBuilder = new UriBuilder(BaseUrl);
            var web = new HtmlWeb();

            UriBuilder = uriBuilder;
            HtmlWeb = web;
        }
        public async Task<List<Lesson>> GetLessonsOfThisDay(int fak_id, int kurs, string groupName, bool IsOddWeek, int dayNumber)
        {
            var lessonsOfThisDay = new List<Lesson>();

            groupName = groupName.ToUpper();

            UriBuilder.Query = $"fak_id={fak_id}&kurs={kurs}&gr={groupName}";
            var doc = HtmlWeb.Load(UriBuilder.Uri);

            string xpath = "";

            if (IsOddWeek)
            {
                xpath = $"//div[@id='collapse_n_1_d_{dayNumber}']/div";
            }
            else xpath = $"//div[@id='collapse_n_2_d_{dayNumber}']/div";

            var dayDiv = doc.DocumentNode.SelectSingleNode(xpath);

            foreach (var lesson in dayDiv.ChildNodes.Where(c => c.Name == "div"))
            {
                var fullLessonName = RecursiveChildFinder(lesson);

                var numberLesson = fullLessonName.Substring(0, 6);
                var stringTimeLesson = fullLessonName.Substring(fullLessonName.IndexOf('(') + 1, fullLessonName.LastIndexOf(')') - fullLessonName.IndexOf('(') - 1);
                var onlyName = fullLessonName.Substring(fullLessonName.IndexOf('/') + 2, fullLessonName.LastIndexOf('/') - fullLessonName.IndexOf('/') - 2);
                var typeLesson = fullLessonName.Substring(fullLessonName.LastIndexOf('/') + 2, fullLessonName.Length - fullLessonName.LastIndexOf('/') - 2);

                var divWhereInfoAboutLesson = lesson
                   .ChildNodes[3]
                   .ChildNodes[1]
                   .ChildNodes
                   .Where(c => c.Name == "p");

                var teacher = divWhereInfoAboutLesson.ElementAt(0).InnerText.Replace("Преподаватель: ", "");
                var audience = divWhereInfoAboutLesson.ElementAt(1).InnerText.Replace("Аудитория: ", "");
                var period = divWhereInfoAboutLesson.ElementAt(2).InnerText.Replace("Период: ", "");
                var remark = divWhereInfoAboutLesson.Last().InnerText.Replace("Примечание: ", "");
                bool isLectureStream = false;
                var percentOfgroup = "100%";

                var nonObviousElem = divWhereInfoAboutLesson.ElementAt(3);
                if (divWhereInfoAboutLesson.Count() > 4)
                {
                    if (nonObviousElem.InnerText.Contains("лекц"))
                    {
                        if (nonObviousElem.InnerText.Contains("Да"))
                            isLectureStream = true;
                        else isLectureStream = false;
                    }
                    else
                    {
                        percentOfgroup = nonObviousElem.InnerText.Substring
                                (nonObviousElem.InnerText.LastIndexOf(':') + 2, nonObviousElem.InnerText.Length - nonObviousElem.InnerText.LastIndexOf(':') - 2);
                    }
                }

                if (teacher == "  ") teacher = null;
                if (remark == "") remark = null;
                lessonsOfThisDay.Add(new Lesson()
                {
                    Teacher = teacher,
                    Audience = audience,
                    Period = period,
                    IsLectureStream = isLectureStream,
                    PercentOfGroup = percentOfgroup,
                    Name = onlyName,
                    Number = numberLesson,
                    Remark = remark,
                    TypeOfLesson = typeLesson,
                    LessonDuration = stringTimeLesson
                });
            }

            return lessonsOfThisDay;

        }
        /// <summary>
        /// Получение списка дней для установки в неделю
        /// </summary>
        /// <param name="fak_id"></param>
        /// <param name="kurs"></param>
        /// <param name="groupName"></param>
        /// <param name="IsOddWeek">true: Odd(нечетная) false: Even(четная)</param>
        /// <returns>Список дней</returns>
        public async Task<List<OneDayTimetable>> GetAllDaysOfWeek(int fak_id, int kurs, string groupName, bool IsOddWeek)
        {
            var daysOfWeek = new List<OneDayTimetable>();

            groupName = groupName.ToUpper();

            UriBuilder.Query = $"fak_id={fak_id}&kurs={kurs}&gr={groupName}";
            var doc = HtmlWeb.Load(UriBuilder.Uri);

            string xpath = "";

            if (IsOddWeek)
            {
                xpath = "//div[@id='collapse_n_1']/div";
            }
            else xpath = "//div[@id='collapse_n_2']/div";

            var WeekDiv = doc.DocumentNode.SelectSingleNode(xpath);

            foreach (var days in WeekDiv.ChildNodes.Where(d => d.Name is "div"))
            {
                var nameOfDay = RecursiveChildFinder(days);
                daysOfWeek.Add(new OneDayTimetable()
                {
                    Day = nameOfDay
                });
            }

            return daysOfWeek;
        }
        public async Task<List<Week>> GetAllWeeksOfGroup(int fak_id, int kurs, string groupName)
        {
            var weeksOfThisGroup = new List<Week>();

            groupName = groupName.ToUpper();

            UriBuilder.Query = $"fak_id={fak_id}&kurs={kurs}&gr={groupName}";
            var doc = HtmlWeb.Load(UriBuilder.Uri);

            var oddWeek = doc.DocumentNode.SelectSingleNode("//div[@id='heading_n_1']");
            var evenWeek = doc.DocumentNode.SelectSingleNode("//div[@id='heading_n_2']");

            if (oddWeek == null && evenWeek == null)
            {
                return weeksOfThisGroup;
            }
            if (oddWeek != null)
            {
                weeksOfThisGroup.Add(new Week()
                {
                    Parity = RecursiveChildFinder(oddWeek)
                });
            }
            if (evenWeek != null)
            {
                weeksOfThisGroup.Add(new Week()
                {
                    Parity = RecursiveChildFinder(evenWeek)
                });
            }
            return weeksOfThisGroup;
        }


        public async Task<List<Group>> GetAllGroupsOfCourse(int fak_id, int kurs)  // мб использовать params?
        {
            var groupsOfThisInstitute = new List<Group>();

            UriBuilder.Query = $"kurs={kurs}&fak_id={fak_id}";
            var doc = HtmlWeb.Load(UriBuilder.Uri);

            var allGroupNodes = doc.DocumentNode.SelectNodes("//select[@id='nal_select_gr']/option");
            allGroupNodes.RemoveAt(0);

            foreach (var group in allGroupNodes)
            {
                groupsOfThisInstitute.Add(new Group()
                {
                    Name = group.InnerText
                });
            }

            return groupsOfThisInstitute;
        }

        public async Task<List<Course>> GetAllCoursesOfInstitute(int fak_id)
        {
            var courses = new List<Course>();

            var doc = HtmlWeb.Load(UriBuilder.Uri);

            var allCoursesOfThisInstitute = doc.DocumentNode.SelectNodes("//select[@id='nal_select_kurs']/option");
            allCoursesOfThisInstitute.RemoveAt(0);

            foreach (var course in allCoursesOfThisInstitute)
            {
                courses.Add(new Course
                {
                    Number = int.Parse(course.InnerText)
                });
            }

            return courses;
        }

        public async Task<List<Institute>> GetAllInstitutes()
        {
            var institutes = new List<Institute>();

            var doc = HtmlWeb.Load(UriBuilder.Uri);

            var allInstNodes = doc.DocumentNode.SelectNodes("//select[@id='nal_select_fak_id']/option");

            allInstNodes.RemoveAt(0);

            foreach (var institute in allInstNodes)
            {
                institutes.Add(new Institute()
                {
                    Name = institute.InnerText,
                    Id = int.Parse(institute.Attributes["value"].Value)
                });
            }
            return institutes;
        }









        public int GetSelectedFacWithId(HtmlDocument doc)
        {
            var a = doc.DocumentNode.SelectSingleNode("//select[@id='nal_select_fak_id']/option[@selected='']");
            var b = a.InnerText;
            return int.Parse(a.Attributes["value"].Value);
        }
        private int GetSelectedCourse(HtmlDocument doc)
        {
            return int.Parse(doc.DocumentNode.SelectSingleNode("//select[@id='nal_select_kurs']/option[@selected='']").InnerText);
        }
        private string GetSelectedGroup(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode("//select[@id='nal_select_gr']/option[@selected='']").InnerText;
        }


        private string RecursiveChildFinder(HtmlNode child)
        {
            if (child.Name != "#text" && child.ChildNodes.Count > 1)
                return RecursiveChildFinder(child.ChildNodes
                    .FirstOrDefault(c => c.Name != "#text"));
            return child.InnerText;
        }
    }
}
