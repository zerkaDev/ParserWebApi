using MediatR;

namespace Timetable.Application.Teacher.Queries.GetTeacherTt
{
    public class GetTeacherTt : IRequest<TeacherTtVm>
    {
        public string TeacherFullName { get; set; }
    }
}
