using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Domain;
using Timetable.Domain.Comparers;

namespace Timetable.Application
{
    public class TimetableDbFiller
    {
        ITimetableDbContext Context { get; }
        TimetableParser Parser { get; }
        public TimetableDbFiller(ITimetableDbContext context)
        {
            Context = context;
            Parser = new TimetableParser();
        }

        public async Task RESHALA()
        {
            var allInst = await Parser.GetAllInstitutes();
            try
            {
                foreach (Institute institute in allInst)
                {
                    var allCourses = await Parser.GetAllCoursesOfInstitute(institute.Id);
                    institute.Courses = new List<Course>();
                    institute.Courses.AddRange(allCourses);
                    foreach (Course course in allCourses)
                    {
                        var allGroups = await Parser.GetAllGroupsOfCourse(institute.Id, course.Number);
                        course.Groups = new List<Group>();
                        course.Groups.AddRange(allGroups);
                        foreach (Group group in allGroups)
                        {
                            var allWeeks = await Parser.GetAllWeeksOfGroup(institute.Id, course.Number, group.Name);
                            group.Weeks = new List<Week>();
                            group.Weeks.AddRange(allWeeks);
                            foreach (Week week in allWeeks)
                            {
                                List<OneDayTimetable> daysOndWeek = new();
                                switch (week.Parity)
                                {
                                    case "Нечетная неделя":
                                        daysOndWeek = await Parser.GetAllDaysOfWeek(institute.Id, course.Number, group.Name, true);
                                        break;
                                    case "Четная неделя":
                                        daysOndWeek = await Parser.GetAllDaysOfWeek(institute.Id, course.Number, group.Name, false);
                                        break;
                                }
                                if (daysOndWeek.Count > 0)
                                {
                                    week.OneDayTimetables = new List<OneDayTimetable>();
                                    week.OneDayTimetables.AddRange(daysOndWeek);
                                    foreach (OneDayTimetable day in daysOndWeek)
                                    {
                                        var lessonsOnDay = await Parser.GetLessonsOfThisDay(institute.Id, course.Number, group.Name, true, await ConvertStrDayToIntDay(day.Day));
                                        lessonsOnDay.Sort(new LessonComparer());
                                        day.Lessons = new List<Lesson>();
                                        day.Lessons.AddRange(lessonsOnDay);
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
            Context.Institutes.AddRange(allInst);

            await Context.SaveChangesAsync(CancellationToken.None);
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
