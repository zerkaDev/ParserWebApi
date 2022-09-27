using Timetable.Application.Interfaces;

namespace Timetable.Persistance
{
    public class DbInitializer
    {
        public static void Initialize(ITimetableDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
