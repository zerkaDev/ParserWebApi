using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timetable.Application;
using Timetable.Application.Interfaces;
using Timetable.Domain;
using Timetable.Domain.ExtensionMethods;

namespace Timetable.Persistance
{
    public class TimetableRepository : ITimetableRepository
    {
        private readonly ITimetableDbContext Context;
        public TimetableRepository(ITimetableDbContext context)
        {
            Context = context;
        }
        public Task<IEnumerable<Group>> GetAllGroups()
        {
            return Task.FromResult(Context.Groups as IEnumerable<Group>);
        }

        public Task<IEnumerable<Institute>> GetAllInstitutes()
        {
            return Task.FromResult(Context.Institutes as IEnumerable<Institute>);
        }

        public async Task<Group> GetGroup(string groupName)
        {
            if (!Context.Institutes.Any())
            {
                TimetableDbFiller f = new TimetableDbFiller(Context);
                await f.RESHALA();
            }
            var group = await Context.Groups.Include(e => e.Weeks)
                .ThenInclude(e => e.OneDayTimetables)
                .ThenInclude(e => e.Lessons)
                .FirstOrDefaultAsync(g => g.Name == groupName);

            if (group is null) return null;

            foreach (var week in group.Weeks)
            {
                week.OrderByDay();
            }

            return group;
        }

        public async Task<IEnumerable<Week>> GetWeeksWithTimetable(string group)
        {
            var groupEntity = await Context.Groups.Include(e=>e.Weeks)
                .ThenInclude(e=>e.OneDayTimetables)
                .ThenInclude(e=>e.Lessons)
                .FirstOrDefaultAsync(g => g.Name == group);

            if (groupEntity is null) return null;

            return groupEntity.Weeks;
        }

        public async Task Save()
        {
            await Context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
