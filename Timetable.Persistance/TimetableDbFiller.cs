using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Domain;
using Timetable.Domain.Comparers;
using Timetable.Parsers.KubSAU;
using Timetable.Parsers.KubSTU;

namespace Timetable.Application
{
    public class TimetableDbFiller
    {
        List<ITimetableParser> Parsers { get; }
        public TimetableDbFiller()
        {
            Parsers = new List<ITimetableParser>
            {
                new KubSAUTimetableParser(),
                new KubSTUTimetableParser()
            };
        }
        public async Task<List<Universities>> RESHALA()
        {
            List<Universities> universities = new();
            try
            {
                foreach (ITimetableParser Parser in Parsers)
                {
                    var allInst = await Parser.GetAllInstitutes();
                    var university = new Universities()
                    {
                        Name = Parser.UniversityName,
                        Institutes = new List<Institute>(allInst)
                    };
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
                    universities.Add(university);
                }
            }
            catch (AggregateException err)
            {
                foreach (var errInner in err.InnerExceptions)
                {
                    Debug.WriteLine(errInner); //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if 
                }
            }

            return universities;
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
