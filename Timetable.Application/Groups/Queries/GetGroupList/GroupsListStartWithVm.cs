using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Common.Mappings;
using Timetable.Domain;

namespace Timetable.Application.Groups.Queries.GetGroupList
{
    public class GroupsListStartWithVm
    {
        public IList<GroupLookupDto> Groups { get; set; }
    }
}
