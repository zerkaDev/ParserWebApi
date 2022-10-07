using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Common.Mappings;
using Timetable.Domain;

namespace Timetable.Application.Teachers.Queries.GetTeacherList
{
    public class TeacherLookupDto : IMapWith<Teacher>
    {
        public string Name { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Teacher, TeacherLookupDto>()
                .ForMember(opt => opt.Name,
                e => e.MapFrom(teacher => teacher.FullName));
        }
    }
}
