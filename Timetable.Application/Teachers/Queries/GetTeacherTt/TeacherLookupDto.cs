using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Common.Mappings;
using Timetable.Application.MapperViewModels;
using Timetable.Domain;

namespace Timetable.Application.Teachers.Queries.GetTeacherTt
{
    public class TeacherLookupDto : IMapWith<Week>
    {
        public bool Parity { get; set; }
        public List<DayVm> Days { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Week, TeacherLookupDto>()
                .ForMember(teacherVm => teacherVm.Parity,
                opt => opt.MapFrom(week => week.Parity))
                .ForMember(teacherVm => teacherVm.Days,
                opt => opt.MapFrom(week => week.OneDayTimetables));
        }
    }
}
