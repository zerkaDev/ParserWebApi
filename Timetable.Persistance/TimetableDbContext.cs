using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Domain;
using Timetable.Persistance.EntityTypeConfiguration;

namespace Timetable.Persistance
{
    public class TimetableDbContext : DbContext, ITimetableDbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Week> Weeks { get; set; }
        public DbSet<OneDayTimetable> OneDayTimetables { get; set; }
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public TimetableDbContext(DbContextOptions<TimetableDbContext> options) 
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new GroupConfiguration());
            builder.ApplyConfiguration(new InstituteConfiguration());
            builder.ApplyConfiguration(new LessonConfiguration());
            builder.ApplyConfiguration(new OneDayTimetableConfiguration());
            builder.ApplyConfiguration(new WeekConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
