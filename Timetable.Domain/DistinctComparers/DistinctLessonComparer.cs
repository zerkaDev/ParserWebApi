using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain.DistinctComparers
{
    public class DistinctLessonComparer : IEqualityComparer<Lesson>
    {
        public bool Equals(Lesson x, Lesson y)
        {
            return x.Audience == y.Audience &&
                x.LessonDuration == y.LessonDuration &&
                x.Name == y.Name &&
                x.Teacher == y.Teacher &&
                x.Number == y.Number &&
                x.TypeOfLesson == y.TypeOfLesson;
        }

        public int GetHashCode(Lesson obj)
        {
           return obj.Audience.GetHashCode() ^
                obj.LessonDuration.GetHashCode() ^
                obj.Name.GetHashCode() ^
                obj.Teacher.GetHashCode() ^
                obj.Number.GetHashCode() ^
                obj.TypeOfLesson.GetHashCode();
        }
    }
}
