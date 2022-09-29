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
    public class GroupLookupDto : IMapWith<Group>
    {
        public string Name { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GroupLookupDto>()
                .ForMember(groupVm => groupVm.Name,
                opt => opt.MapFrom(group => group.Name));
        }
    }
}
