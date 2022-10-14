using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain
{
    public class Week
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Returns true if odd (нечетная), false if even (четная)
        /// </summary>
        public bool Parity { get; set; }
        /// <summary>
        /// Расписание на неделю
        /// </summary>
        public List<OneDayTimetable> OneDayTimetables { get; set; }
        public Group Group { get; set; }

    }
}
