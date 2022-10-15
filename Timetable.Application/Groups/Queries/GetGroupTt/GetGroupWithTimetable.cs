using System;
using MediatR;

namespace Timetable.Application.Queries.Groups
{
    public class GetGroupWithTimetable : IRequest<GroupVm>
    {
        public string University { get; set; }
        public string Name { get; set; }
    }
}
