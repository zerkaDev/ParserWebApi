using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timetable.Domain;

namespace Timetable.Persistance.EntityTypeConfiguration
{
    class OneDayTimetableConfiguration : IEntityTypeConfiguration<OneDayTimetable>
    {
        public void Configure(EntityTypeBuilder<OneDayTimetable> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasOne(f => f.Week).WithMany(g => g.OneDayTimetables).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(l => l.Lessons).WithOne(o => o.OneDayTimetable).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
