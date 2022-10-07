using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Teachers.Queries.GetTeachersFromGroup
{
    public class GetTeachersFromGroup : IRequest<TeachersFromGroupVm>
    {
        public string GroupName { get; set; }
    }
}
