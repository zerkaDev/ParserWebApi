using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Common.Mappings;
using Timetable.Domain;

namespace Timetable.Application.Queries.Groups
{
    public class GroupVm : IMapWith<Group>
    {
        public string Name { get; set; }
        public List<Week> Weeks { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Group, GroupVm>()
                .ForMember(groupVm => groupVm.Name,
                opt => opt.MapFrom(note => note.Name))
                .ForMember(groupVm => groupVm.Weeks,
                opt => opt.MapFrom(note => note.Weeks));
        }

    }
}
