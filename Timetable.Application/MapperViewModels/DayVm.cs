using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Common.Mappings;
using Timetable.Domain;

namespace Timetable.Application.MapperViewModels
{
    public class DayVm : IMapWith<OneDayTimetable>
    {
        public string Day { get; set; }
        public List<LessonVm> LessonVms { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<OneDayTimetable, DayVm>()
                .ForMember(dayVm => dayVm.Day,
                opt => opt.MapFrom(day => day.Day))
                .ForMember(dayVm => dayVm.LessonVms,
                opt => opt.MapFrom(day => day.Lessons));
        }
    }
}
