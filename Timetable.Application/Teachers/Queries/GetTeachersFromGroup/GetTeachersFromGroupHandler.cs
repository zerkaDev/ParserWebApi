using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Domain;

namespace Timetable.Application.Teachers.Queries.GetTeachersFromGroup
{
    public class GetTeachersFromGroupHandler : IRequestHandler<GetTeachersFromGroup, TeachersFromGroupVm>
    {
        private readonly ITimetableDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetTeachersFromGroupHandler(ITimetableDbContext dbContext,
            IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<TeachersFromGroupVm> Handle(GetTeachersFromGroup request, CancellationToken cancellationToken)
        {
            var group = await _dbContext.Groups.Include(e => e.Weeks)
                .ThenInclude(e => e.OneDayTimetables)
                .ThenInclude(e => e.Lessons)
                .ThenInclude(e => e.Teacher)
                .FirstOrDefaultAsync(g => g.Name == request.GroupName.ToUpper(), cancellationToken);

            if (group is null) return null;

            var teachers = new List<TeacherLookupDto>();

            foreach (Week week in group.Weeks)
            {
                foreach (OneDayTimetable days in week.OneDayTimetables)
                {
                    foreach (Lesson lesson in days.Lessons)
                    {
                        if (lesson.Teacher is null) continue;
                        if (teachers.FirstOrDefault(t=>t?.Name == lesson.Teacher?.FullName) is null)
                            teachers.Add(_mapper.Map<TeacherLookupDto>(lesson.Teacher));
                    }
                }
            }

            return new TeachersFromGroupVm() { Teachers = new List<TeacherLookupDto>(teachers) };
        }
    }
}
