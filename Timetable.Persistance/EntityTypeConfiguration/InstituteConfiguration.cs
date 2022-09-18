using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timetable.Domain;

namespace Timetable.Persistance.EntityTypeConfiguration
{
    public class InstituteConfiguration : IEntityTypeConfiguration<Institute>
    {
        public void Configure(EntityTypeBuilder<Institute> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasMany(c => c.Courses).WithOne(i => i.Institute);
            builder.Property(i => i.Id).ValueGeneratedNever();
        }
    }
}
