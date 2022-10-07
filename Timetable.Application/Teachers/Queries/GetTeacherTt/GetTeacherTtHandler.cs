using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Application.MapperViewModels;
using Timetable.Domain;
using Timetable.Domain.Comparers;
using Timetable.Domain.DistinctComparers;
using Timetable.Domain.ExtensionMethods;

namespace Timetable.Application.Teachers.Queries.GetTeacherTt
{
    public class GetTeacherTtHandler : IRequestHandler<GetTeacherTt, TeacherTtVm>
    {
        private readonly ITimetableDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetTeacherTtHandler(ITimetableDbContext context,
            IMapper mapper) => (_dbContext, _mapper) = (context, mapper);
        public async Task<TeacherTtVm> Handle(GetTeacherTt request, CancellationToken cancellationToken)
        {
            // Custom Distinct used to remove repeats lesson (2 groups maybe have a one lessson, but they has different id, but teacher isnt)

            var teacherWithLesson = await _dbContext.Teachers.Include(e => e.Lessons)
                .ThenInclude(e => e.OneDayTimetable)
                .ThenInclude(e => e.Week)
                .FirstOrDefaultAsync(name => name.FullName == request.TeacherFullName, cancellationToken);

            if (teacherWithLesson is null)
                return null;

            var lessonsDistinct = teacherWithLesson.Lessons
                .Distinct(new DistinctLessonComparer())
                .GroupBy(g =>
                    new { g.OneDayTimetable.Day, g.OneDayTimetable.Week.Parity });

            // Now, because we arent have a teacher model, we need to compose a timetable for this one [teacher have 2 weeks]
            // Нельзя вынести в отедлньый метод так как 1 параметр это анонимный тип

            var timetable = new List<Week>();

            foreach (var groupItem in lessonsDistinct)
            {
                OneDayTimetable day = new () { Day = groupItem.Key.Day, Lessons = new List<Lesson>() };

                var week = timetable.FirstOrDefault(w => w.Parity == groupItem.Key.Parity);

                if (week == null) { week = new Week() { Parity = groupItem.Key.Parity, OneDayTimetables = new List<OneDayTimetable>() }; timetable.Add(week); }

                if (groupItem.Key.Parity == week.Parity)
                {
                    foreach (var lesson in groupItem)
                    {
                        lesson.OneDayTimetable = null;
                        lesson.Teacher = null;
                        day.Lessons.Add(lesson);
                    }
                    day.Lessons.Sort(new LessonComparer());
                    week.OneDayTimetables.Add(day);
                }
            }

            var weekVms = new List<WeekVm>();

            foreach (var week in timetable)
            {
                week.OrderByDay();
                weekVms.Add(_mapper.Map<WeekVm>(week));
            }

            return new TeacherTtVm() { TeacherFullName = request.TeacherFullName, TeacherWeeks = weekVms };
        }
    }
}
