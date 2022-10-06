using MediatR;

namespace Timetable.Application.Teachers.Queries.GetTeacherTt
{
    public class GetTeacherTt : IRequest<TeacherTtVm>
    {
        public string TeacherFullName { get; set; }
    }
}
