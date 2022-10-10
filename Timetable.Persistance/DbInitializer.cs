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
        public async static Task Initialize(ITimetableDbContext context, TelegramBot bot)
        {
            _bot = bot;
            await _bot.SendMessageAboutDbInitializerStart();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            await _bot.SendMessageAboutDbWasCreated();
            if (!context.Universities.Any())
            {
                await _bot.SendMessageAboutDbHasntValues();
                await AddTimetable(context);
            }

            await _bot.SendMessageAboutDbInitializerStops();
        }
        public async static Task AddTimetable(ITimetableDbContext context)
        {
            await _bot.SendMessageAboutReshalaStartsWorking();

            TimetableDbFiller filler = new();
            var univ = await filler.RESHALA();
            context.Universities.Add(univ);
            await context.SaveChangesAsync(System.Threading.CancellationToken.None);

            await _bot.SendMessageAboutReshalaStopsWorking();
        }
    }
}
