using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Domain;
using Timetable.Domain.Comparers;
using Timetable.Domain.DistinctComparers;
using Timetable.Domain.ExtensionMethods;

namespace Timetable.Application.Teacher.Queries.GetTeacherTt
{
    public class GetTeacherTtHandler : IRequestHandler<GetTeacherTt, TeacherTtVm>
    {
        private readonly ITimetableDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetTeacherTtHandler(ITimetableDbContext context,
            IMapper mapper) => (_dbContext, _mapper) = (context, mapper);
        public Task<TeacherTtVm> Handle(GetTeacherTt request, CancellationToken cancellationToken)
        {
            // Custom Distinct used to remove repeats lesson (2 groups maybe have a one lessson, but they has different id, but teacher isnt)

            var teacherLessons = _dbContext.Lessons.Include(e => e.OneDayTimetable)
                .ThenInclude(e => e.Week)
                .Where(name => name.Teacher == request.TeacherFullName)
                .ToList()
                .Distinct(new DistinctLessonComparer())
                .GroupBy(g =>
                    new { g.OneDayTimetable.Day, g.OneDayTimetable.Week.Parity });

            // Now, because we arent have a teacher model, we need to compose a timetable for this one [teacher have 2 weeks]
            // Не могу разделить метод, так как нельзя создать метод, принимающий IGrouping<'a,.> ;(

            var oddWeek = new Week() { Parity = "Нечетная неделя", OneDayTimetables = new List<OneDayTimetable>() };
            var evenWeek = new Week() { Parity = "Четная неделя", OneDayTimetables = new List<OneDayTimetable>() };

            var timetable = new List<Week>();
            #region Bad
            foreach (var groupItem in teacherLessons)
            {
                OneDayTimetable day = new OneDayTimetable() { Day = groupItem.Key.Day, Lessons = new List<Lesson>() };
                if (groupItem.Key.Parity == oddWeek.Parity)
                {
                    foreach (var lesson in groupItem)
                    {
                        lesson.OneDayTimetable = null;
                        day.Lessons.Add(lesson);
                    }
                    day.Lessons.Sort(new LessonComparer());
                    oddWeek.OneDayTimetables.Add(day);
                }
                if (groupItem.Key.Parity == evenWeek.Parity)
                {
                    foreach (var lesson in groupItem)
                    {
                        lesson.OneDayTimetable = null;
                        day.Lessons.Add(lesson);
                    }
                    day.Lessons.Sort(new LessonComparer());
                    evenWeek.OneDayTimetables.Add(day);
                }
            }

            oddWeek.OrderByDay();
            evenWeek.OrderByDay();
            timetable.Add(oddWeek);
            timetable.Add(evenWeek);
            #endregion

            var finalTt = timetable
                .AsQueryable()
                .ProjectTo<TeacherLookupDto>(_mapper.ConfigurationProvider).ToList();
            return Task.FromResult(new TeacherTtVm() { TeacherWeeks = finalTt });
        }
    }
}
