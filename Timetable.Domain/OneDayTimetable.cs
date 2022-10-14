using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain
{
    public class OneDayTimetable
    {
        /// <summary>
        /// Уникальный ид
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// День недели
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// Список предметов
        /// </summary>
        public List<Lesson> Lessons { get; set; }
        public Week Week { get; set; }

    }
}
