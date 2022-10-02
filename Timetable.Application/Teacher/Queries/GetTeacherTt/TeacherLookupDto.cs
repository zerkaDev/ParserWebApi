using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Common.Mappings;
using Timetable.Domain;

namespace Timetable.Application.Teacher.Queries.GetTeacherTt
{
    public class TeacherLookupDto : IMapWith<Week>
    {

        public string Parity { get; set; }
        public List<OneDayTimetable> Days { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Week, TeacherLookupDto>()
                .ForMember(teacherVm => teacherVm.Days,
                opt => opt.MapFrom(week => week.OneDayTimetables))
                .ForMember(teacherVm => teacherVm.Parity,
                opt => opt.MapFrom(week => week.Parity));
        }
    }
}
