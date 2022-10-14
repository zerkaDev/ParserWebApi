using System;
using System.Collections.Generic;

namespace Timetable.Domain
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
