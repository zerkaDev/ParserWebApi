using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Domain;

namespace Timetable.Parsers.KubSTU
{
    public class KubSTUTimetableParser : ITimetableParser
    {
        public string UniversityName { get; } = "KubSTU";
        string BaseUrl { get; } = "https://elkaf.kubstu.ru/timetable/default/time-table-student-ofo";
        UriBuilder UriBuilder { get; }
        HtmlWeb HtmlWeb { get; }
        HtmlDocument LastRequestDocument { get; set; }
        // I think it is the normal way to store uniqal teacher in the parses-works time instead of use db context
        List<Teacher> Teachers { get; set; }

        public KubSTUTimetableParser()
        {
            this.UriBuilder = new UriBuilder(BaseUrl);
            HtmlWeb = new HtmlWeb();
            Teachers = new List<Teacher>();
        }

        // TODO : Разделяй и властвуй...
        public Task<List<Lesson>> GetLessonsOfThisDay(bool IsOddWeek, int dayNumber)
        {
            var lessonsOfThisDay = new List<Lesson>();

            string xpath = "";

            if (IsOddWeek)
            {
                xpath = $"//div[@id='collapse_n_1_d_{dayNumber}']/div";
            }
            else xpath = $"//div[@id='collapse_n_2_d_{dayNumber}']/div";

            var dayDiv = LastRequestDocument.DocumentNode.SelectSingleNode(xpath);

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

                var teacherName = divWhereInfoAboutLesson.ElementAt(0).InnerText.Replace("Преподаватель: ", "");
                var audience = divWhereInfoAboutLesson.ElementAt(1).InnerText.Replace("Аудитория: ", "");

                Domain.Teacher teacherModel;

                if (teacherName == "  ") 
                    teacherModel = null;
                else
                {
                    teacherModel = Teachers.FirstOrDefault(t => t.FullName == teacherName);
                    if (teacherModel is null) Teachers.Add(new Teacher()
                    {
                        FullName = teacherName,
                        Lessons = new List<Lesson>()
                    });
                }

                var oneLesson = new Lesson()
                {
                    Teacher = teacherModel,
                    Audience = audience,
                    Name = onlyName,
                    Number = numberLesson,
                    TypeOfLesson = typeLesson,
                    LessonDuration = stringTimeLesson
                };

                lessonsOfThisDay.Add(oneLesson);

                if (teacherModel != null) teacherModel.Lessons.Add(oneLesson);
            }

            return Task.FromResult(lessonsOfThisDay);

        }
        /// <summary>
        /// Получение списка дней для установки в неделю
        /// </summary>
        /// <param name="fak_id"></param>
        /// <param name="kurs"></param>
        /// <param name="groupName"></param>
        /// <param name="IsOddWeek">true: Odd(нечетная) false: Even(четная)</param>
        /// <returns>Список дней</returns>
        public Task<List<OneDayTimetable>> GetAllDaysOfWeek(bool IsOddWeek)
        {
            var daysOfWeek = new List<OneDayTimetable>();

            string xpath = "";

            if (IsOddWeek)
                xpath = "//div[@id='collapse_n_1']/div";
            else xpath = "//div[@id='collapse_n_2']/div";

            var WeekDiv = LastRequestDocument.DocumentNode.SelectSingleNode(xpath);

            foreach (var days in WeekDiv.ChildNodes.Where(d => d.Name is "div"))
            {
                var nameOfDay = RecursiveChildFinder(days);
                daysOfWeek.Add(new OneDayTimetable()
                {
                    Day = nameOfDay
                });
            }

            return Task.FromResult(daysOfWeek);
        }

        public Task<List<Week>> GetAllWeeksOfGroup(int fak_id, int kurs, string groupName)
        {
            var weeksOfThisGroup = new List<Week>();

            groupName = groupName.ToUpper();

            UriBuilder.Query = $"fak_id={fak_id}&kurs={kurs}&gr={groupName}";
            var doc = HtmlWeb.Load(UriBuilder.Uri);
            LastRequestDocument = doc;

            for (int i = 1; i <= 2; i++)
            {
                var week = doc.DocumentNode.SelectSingleNode($"//div[@id='heading_n_{i}']");
                if (week != null) weeksOfThisGroup.Add(new Week()
                {
                    Parity = RecursiveChildFinder(week) is "Нечетная неделя"
                });
            }
            if (weeksOfThisGroup.Count == 0)
                return Task.FromResult(weeksOfThisGroup);

            return Task.FromResult(weeksOfThisGroup);
        }

        public Task<List<Group>> GetAllGroupsOfCourse(int fak_id, int kurs)
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

            return Task.FromResult(groupsOfThisInstitute);
        }

        public Task<List<Course>> GetAllCoursesOfInstitute()
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

            return Task.FromResult(courses);
        }

        public Task<List<Institute>> GetAllInstitutes()
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
            return Task.FromResult(institutes);
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
