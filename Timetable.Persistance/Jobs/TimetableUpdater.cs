using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application;
using Timetable.Application.Interfaces;

namespace Timetable.Persistance.Jobs
{
    public class TimetableUpdater : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TimetableUpdater(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ITimetableDbContext>();
            await DbInitializer.UpdateTimetable(dbContext);
        }
    }
}
