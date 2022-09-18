using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain
{
    public class Week
    {
        public int Id { get; set; }
        /// <summary>
        /// Четность
        /// </summary>
        public string Parity { get; set; }
        /// <summary>
        /// Расписание на неделю
        /// </summary>
        public List<OneDayTimetable> OneDayTimetables { get; set; }
        public Group Group { get; set; }

    }
}
