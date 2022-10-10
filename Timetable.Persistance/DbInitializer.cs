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


            await AddTimetable(context);


            await _bot.SendMessageAboutDbInitializerStops();
        }
        public async static Task AddTimetable(ITimetableDbContext context)
        {
            await _bot.SendMessageAboutReshalaStartsWorking();

            TimetableDbFiller filler = new();
            var univ = await filler.RESHALA();

            if (!context.Universities.Any())
            {
                await _bot.SendMessageAboutDbHasntValues();
                await context.Universities.AddAsync(univ);
            }
            else
                context.Universities.Update(univ);

            await context.SaveChangesAsync(System.Threading.CancellationToken.None);

            await _bot.SendMessageAboutReshalaStopsWorking();
        }
    }
}
