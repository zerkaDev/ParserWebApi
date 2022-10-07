using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Teachers.Queries.GetTeacherList
{
    public class TeacherListStartsWithVm
    {
        public IList<TeacherLookupDto> Teachers { get; set; }
    }
}
