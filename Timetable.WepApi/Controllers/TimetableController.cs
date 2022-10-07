using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timetable.Application.Groups.Queries.GetGroupList;
using Timetable.Application.Interfaces;
using Timetable.Application.Queries.Groups;
using Timetable.Application.Teachers.Queries.GetTeacherList;
using Timetable.Application.Teachers.Queries.GetTeacherTt;
using Timetable.Domain;
using Timetable.Domain.DistinctComparers;

namespace Timetable.WepApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimetableController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TimetableController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("groups/{symbols}")]
        public async Task<JsonResult> GetGroupsStartingWith(string symbols)
        {
            var query = new GetGroupsListStartWith() { Symbols = symbols.ToUpper() };
            var vm = await _mediator.Send(query);
            return new JsonResult(vm) { StatusCode = Ok().StatusCode };
        }
        [HttpGet("teachers/{symbols}")]
        public async Task<JsonResult> GetTeachersStartingWith(string symbols)
        {
            var query = new GetTeacherListStartsWith() { Symbols = symbols };
            var vm = await _mediator.Send(query);
            return new JsonResult(vm) { StatusCode = Ok().StatusCode };
        }
        [HttpGet("teachertt/{fio}")]
        public async Task<ActionResult<TeacherTtVm>> GetTeacherTimetable(string fio)
        {
            var query = new GetTeacherTt() { TeacherFullName = fio };
            var vm = await _mediator.Send(query);
            return new JsonResult(vm) { StatusCode = Ok().StatusCode};
            
        }
        [HttpGet("{groupName}")]
        public async Task<ActionResult<GroupVm>> GetGroupTt(string groupName)
        {
            var query = new GetGroupWithTimetable() { Name = groupName.ToUpper() };
            var vm = await _mediator.Send(query);
            if (vm is null) return NoContent();
            return new JsonResult(vm) { StatusCode = Ok().StatusCode };
        }

    }
}
