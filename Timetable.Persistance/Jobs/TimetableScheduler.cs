using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;

namespace Timetable.Persistance.Jobs
{
    public class TimetableScheduler
    {
        public static async void Start(IServiceProvider serviceProvider)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = serviceProvider.GetService<JobFactory>();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<TimetableUpdater>().Build();

            DateTime todayMidnight = DateTime.Today.AddHours(24);

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger1", "group1")  
                //.StartAt(new DateTimeOffset(DateTime.Now.AddMinutes(15))) // идентифицируем триггер с именем и группой
                .StartAt(new DateTimeOffset(todayMidnight))  // запуск каждую полночь
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithIntervalInHours(24)          // через сутки
                    .RepeatForever())                   // бесконечное повторение
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
        
        
    }
}
