using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Teacher.Queries.GetTeacherTt
{
    public class TeacherTtVm
    {
        public IList<TeacherLookupDto> TeacherWeeks { get; set; }
    }
}
