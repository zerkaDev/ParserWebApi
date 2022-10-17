using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Domain.ExtensionMethods;

namespace Timetable.Application.Queries.Groups
{
    public class GetGroupWithTimetableHandler :
        IRequestHandler<GetGroupWithTimetable, GroupVm>
    {
        private readonly ITimetableDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetGroupWithTimetableHandler(ITimetableDbContext dbContext,
            IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);
        public async Task<GroupVm> Handle(GetGroupWithTimetable request, CancellationToken cancellationToken)
        {
            var universitiy = await _dbContext.Universities
                .FirstOrDefaultAsync(u => u.Name.ToLower() == request.University.ToLower());

            if (universitiy is null) return null;

            var group = await _dbContext.Groups
                .Include(e => e.Course)
                .ThenInclude(e => e.Institute)
                .ThenInclude(e => e.University)
                .FirstOrDefaultAsync(g => g.Name == request.Name, cancellationToken);

            // Некрасиво

            if (group == null || group.Name != request.Name || group.Course.Institute.University.Name.ToLower() != request.University.ToLower())
            {
                return null;
            }

            group = await _dbContext.Groups.
                Include(e => e.Weeks.OrderBy(s=>s.Parity))
                .ThenInclude(e => e.OneDayTimetables)
                .ThenInclude(e => e.Lessons.OrderBy(s => s.Number))
                .ThenInclude(e => e.Teacher)
                .FirstOrDefaultAsync(g => g.Name == request.Name, cancellationToken);

            foreach (var week in group.Weeks)
                week.OrderByDay();

            return _mapper.Map<GroupVm>(group);
        }
    }
}
