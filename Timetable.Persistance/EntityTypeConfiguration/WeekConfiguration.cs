
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timetable.Domain;

namespace Timetable.Persistance.EntityTypeConfiguration
{
    public class WeekConfiguration : IEntityTypeConfiguration<Week>
    {
        public void Configure(EntityTypeBuilder<Week> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasOne(g => g.Group).WithMany(w => w.Weeks).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(d => d.OneDayTimetables).WithOne(w => w.Week).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
