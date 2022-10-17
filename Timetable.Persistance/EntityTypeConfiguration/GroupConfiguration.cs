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
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(k => k.Name);
            builder.HasOne(c => c.Course).WithMany(g => g.Groups).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(w=>w.Weeks).WithOne(g=>g.Group).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
