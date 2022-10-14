using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Domain;

namespace Timetable.Application.Interfaces
{
    public interface ITimetableParser
    {
        public string UniversityName { get; }
        public Task<List<Lesson>> GetLessonsOfThisDay(bool IsOddWeek, int dayNumber);
        public Task<List<OneDayTimetable>> GetAllDaysOfWeek(bool IsOddWeek);
        public Task<List<Week>> GetAllWeeksOfGroup(int fak_id, int kurs, string groupName);
        public Task<List<Group>> GetAllGroupsOfCourse(int fak_id, int kurs);
        public Task<List<Course>> GetAllCoursesOfInstitute();
        public Task<List<Institute>> GetAllInstitutes();
    }
}
