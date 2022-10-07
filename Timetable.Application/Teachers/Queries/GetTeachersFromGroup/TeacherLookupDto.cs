using AutoMapper;
using Timetable.Application.Common.Mappings;
using Timetable.Domain;

namespace Timetable.Application.Teachers.Queries.GetTeachersFromGroup
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
