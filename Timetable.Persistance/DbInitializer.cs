using System.Linq;
using System.Threading.Tasks;
using Timetable.Application;
using Timetable.Application.Interfaces;

namespace Timetable.Persistance
{
    public class DbInitializer
    {
        public async static void Initialize(ITimetableDbContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Universities.Any())
            {
                await AddTimetable(context);
            }
        }
        public async static Task AddTimetable(ITimetableDbContext context)
        {
            TimetableDbFiller filler = new ();
            var univ = await filler.RESHALA();
            context.Universities.Add(univ);
            await context.SaveChangesAsync(System.Threading.CancellationToken.None);
        }
    }
}
