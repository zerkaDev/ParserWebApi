using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Domain;

namespace Timetable.Persistance.EntityTypeConfiguration
{
    class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(k => k.Id);
            builder.HasOne(g => g.OneDayTimetable).WithMany(l => l.Lessons);
            builder.HasOne(t => t.Teacher).WithMany(l => l.Lessons);
            builder.Property(p => p.TeacherId).IsRequired(false);
        }
    }
}
