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
    public class WeekVm : IMapWith<Week>
    {
        /// <summary>
        /// Returns true if odd, false if even
        /// </summary>
        public bool Parity{ get; set; }
        public List<DayVm> DayVms { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Week, WeekVm>()
                .ForMember(weekVm => weekVm.Parity,
                opt => opt.MapFrom(week => week.Parity))
                .ForMember(weekVm => weekVm.DayVms,
                opt => opt.MapFrom(week => week.OneDayTimetables));
        }
    }
}
