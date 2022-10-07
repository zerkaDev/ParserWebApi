using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.MapperViewModels;

namespace Timetable.Application.Teachers.Queries.GetTeacherTt
{
    public class TeacherTtVm
    {
        public string TeacherFullName { get; set; }
        public IList<WeekVm> TeacherWeeks { get; set; }
    }
}
