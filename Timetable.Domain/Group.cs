
using System.Collections.Generic;

namespace Timetable.Domain
{
    public class Group
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<Week> Weeks { get; set; } 
    }
}
