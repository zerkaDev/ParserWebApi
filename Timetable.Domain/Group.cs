using System;
using System.Collections.Generic;

namespace Timetable.Domain
{
    public class Group
    {
        /// <summary>
        /// Название [надеемся что пока не будет в двух разных униках одинакого названия]
        /// </summary>
        public string Name { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public List<Week> Weeks { get; set; } 
    }
}
