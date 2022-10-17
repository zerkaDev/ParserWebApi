using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Informer;
using Timetable.Application;
using Timetable.Application.Interfaces;

namespace Timetable.Persistance
{
    public class DbInitializer
    {
        private static TelegramBot _bot;
        public async static Task Initialize(IServiceProvider service)
        {
            var context = service.GetRequiredService<ITimetableDbContext>();
            _bot = service.GetRequiredService<TelegramBot>();

            await _bot.SendMessageAboutDbInitializerStart();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            await _bot.SendMessageAboutDbWasCreated();
            if (context.Universities.Count() != 2)
                await AddTimetable(context);

            await _bot.SendMessageAboutDbInitializerStops();
        }
        public async static Task AddTimetable(ITimetableDbContext context)
        {
            await _bot.SendMessageAboutReshalaStartsWorking();

            TimetableDbFiller filler = new();
            var allUniversities = await filler.RESHALA();

            await _bot.SendMessageAboutDbHasntValues();

            foreach (var univ in allUniversities)
                await context.Universities.AddAsync(univ);

            await context.SaveChangesAsync(System.Threading.CancellationToken.None);

            await _bot.SendMessageAboutReshalaStopsWorking();
        }
        public async static Task UpdateTimetable(ITimetableDbContext context)
        {
            await _bot.SendMessageAboutReshalaStartsWorking();

            TimetableDbFiller filler = new();
            var allUniversities = await filler.RESHALA();


            foreach (var univ in allUniversities)
            {
                var univEntity = context.Universities
                    .FirstOrDefault(e => e.Name == univ.Name);

                if (univEntity != null)
                {
                    context.Universities.Remove(univEntity);
                    await context.SaveChangesAsync(System.Threading.CancellationToken.None);
                }

                await context.Universities.AddAsync(univ);
            }

            await context.SaveChangesAsync(System.Threading.CancellationToken.None);

            await _bot.SendMessageAboutReshalaStopsWorking();
        }
    }
}
