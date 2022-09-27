
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain
{
    public class Institute
    {
        public int Id { get; set; }
        /// <summary>
        /// Название направления
        /// </summary>
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
        public University University { get; set; }
    }
}
