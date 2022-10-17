using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Groups.Queries.GetGroupList
{
    public class GetGroupsListStartWith : IRequest<GroupsListStartWithVm>
    {
        public string UniversityName { get; set; }
        public string Symbols { get; set; }
    }
}
