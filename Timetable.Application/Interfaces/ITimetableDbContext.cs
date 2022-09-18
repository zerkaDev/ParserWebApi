using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timetable.Domain;

namespace Timetable.Application.Interfaces
{
    public interface ITimetableDbContext
    {
        DatabaseFacade Database { get; }
        DbSet<Course> Courses { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<Week> Weeks { get; set; }
        DbSet<OneDayTimetable> OneDayTimetables { get; set; }
        DbSet<Institute> Institutes { get; set; }
        DbSet<Lesson> Lessons { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
