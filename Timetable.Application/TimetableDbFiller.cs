using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Timetable.Domain;
using Timetable.Domain.Comparers;

namespace Timetable.Application
{
    public class TimetableDbFiller
    {
        TimetableParser Parser { get; }
        public TimetableDbFiller()
        {
            Parser = new TimetableParser();
        }
        public async Task<Universities> RESHALA()
        {
            Universities KubSTU = new() { Name = "Кубанский государственный технологический университет." };
            var allInst = await Parser.GetAllInstitutes();
            KubSTU.Institutes = new List<Institute>(allInst);
            try
            {
                foreach (Institute institute in allInst)
                {
                    var allCourses = await Parser.GetAllCoursesOfInstitute();
                    institute.Courses = new List<Course>(allCourses);
                    foreach (Course course in allCourses)
                    {
                        var allGroups = await Parser.GetAllGroupsOfCourse(institute.Id, course.Number);
                        course.Groups = new List<Group>(allGroups);
                        foreach (Group group in allGroups)
                        {
                            var allWeeks = await Parser.GetAllWeeksOfGroup(institute.Id, course.Number, group.Name);
                            group.Weeks = new List<Week>(allWeeks);
                            foreach (Week week in allWeeks)
                            {
                                List<OneDayTimetable> daysOndWeek = new();
                                daysOndWeek = await Parser.GetAllDaysOfWeek(week.Parity);
                                if (daysOndWeek.Count > 0)
                                {
                                    week.OneDayTimetables = new List<OneDayTimetable>(daysOndWeek);
                                    foreach (OneDayTimetable day in daysOndWeek)
                                    {
                                        var lessonsOnDay = await Parser.GetLessonsOfThisDay(week.Parity, await ConvertStrDayToIntDay(day.Day));
                                        lessonsOnDay.Sort(new LessonComparer());
                                        day.Lessons = new List<Lesson>(lessonsOnDay);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (AggregateException err)
            {
                foreach (var errInner in err.InnerExceptions)
                {
                    Debug.WriteLine(errInner); //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                }
            }
            return KubSTU;
        }
        private static Task<int> ConvertStrDayToIntDay(string day)
        {
            var daiInt = 0;
            switch (day)
            {
                case "Понедельник":
                    daiInt = 1;
                    break;
                case "Вторник":
                    daiInt = 2;
                    break;
                case "Среда":
                    daiInt = 3;
                    break;
                case "Четверг":
                    daiInt = 4;
                    break;
                case "Пятница":
                    daiInt = 5;
                    break;
                case "Суббота":
                    daiInt = 6;
                    break;
            }
            return Task.FromResult(daiInt);
        }
    }
}
