using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain.Comparers
{
    public class LessonComparer : IComparer<Lesson>
    {
        int IComparer<Lesson>.Compare(Lesson x, Lesson y)
        {
            if (x == null || y == null)
            {
                return 0;
            }
            return x.Number.CompareTo(y.Number);
        }
    }
}
