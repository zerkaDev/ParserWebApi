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
    public class UniversityConfiguration : IEntityTypeConfiguration<Universities>
    {
        public void Configure(EntityTypeBuilder<Universities> builder)
        {
            builder.HasKey(k => k.Name);
            builder.HasMany(i => i.Institutes).WithOne(u => u.University);
        }
    }
}
