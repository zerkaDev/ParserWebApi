using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Timetable.Application.Common.Mappings;
using Timetable.Domain;
namespace Timetable.Application.MapperViewModels
{
    public class LessonVm : IMapWith<Lesson>
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string LessonDuration { get; set; }
        public string TypeOfLesson { get; set; }
        public string Teacher { get; set; }
        public string Audience { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Lesson, LessonVm>()
                .ForMember(lessonVm => lessonVm.Name,
                opt => opt.MapFrom(lesson => lesson.Name))
                .ForMember(lessonVm => lessonVm.Number,
                opt => opt.MapFrom(lesson => lesson.Number)).
                ForMember(lessonVm => lessonVm.LessonDuration,
                opt => opt.MapFrom(lesson => lesson.LessonDuration)).
                ForMember(lessonVm => lessonVm.TypeOfLesson,
                opt => opt.MapFrom(lesson => lesson.TypeOfLesson)).
                ForMember(lessonVm => lessonVm.Audience,
                opt => opt.MapFrom(lesson => lesson.Audience)).
                ForMember(lessonVm => lessonVm.Teacher,
                opt => opt.MapFrom(lesson => lesson.Teacher.FullName));
        }
    }
}
