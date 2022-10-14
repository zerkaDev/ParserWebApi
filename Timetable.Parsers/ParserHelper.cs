using System;
using System.Collections.Generic;
using System.Linq;
using Timetable.Domain;

namespace Timetable.Parsers
{
    internal class ParserHelper
    {
        private List<Teacher> Teachers { get; set; }
        public ParserHelper()
        {
            Teachers = new List<Teacher>();
        }
        internal OneDayTimetable CreateDay(string day)
        {
            return new OneDayTimetable()
            {
                Id = Guid.NewGuid(),
                Day = day
            };
        }
        internal Course CreateCourse(int number)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Number = number
            };
        }
        internal Week CreateWeek(bool IsOdd)
        {
            return new Week()
            {
                Id = Guid.NewGuid(),
                Parity = IsOdd
            };
        }
        internal Teacher GetTeacherOrDefault(string teacherName)
        {
            Teacher teacher = Teachers.FirstOrDefault(t => t.FullName == teacherName);

            if (teacher is null)
            {
                teacher = new Teacher()
                {
                    Id = Guid.NewGuid(),
                    FullName = teacherName
                    //Lessons = new List<Lesson>()
                };
                Teachers.Add(teacher);
                return teacher;
            }
            return teacher;
        }
        internal Lesson CreateLesson(
            string audience,
            string duration,
            string name,
            string number,
            string typeOfLesson,
            Teacher teacher)
        {
            return new Lesson()
            {
                Id = Guid.NewGuid(),
                Audience = audience,
                LessonDuration = duration,
                Name = name,
                Number = number,
                TypeOfLesson = typeOfLesson,
                Teacher = teacher
            };
        }
    }
}

