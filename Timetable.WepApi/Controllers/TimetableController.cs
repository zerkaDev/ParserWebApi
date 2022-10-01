using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timetable.Application.Groups.Queries.GetGroupList;
using Timetable.Application.Interfaces;
using Timetable.Application.Queries.Groups;
using Timetable.Domain;

namespace Timetable.WepApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimetableController : ControllerBase
    {
        ITimetableRepository Repo { get; }
        private IMediator _mediator;
        public TimetableController(ITimetableRepository repo, IMediator mediator)
        {
            Repo = repo;
            _mediator = mediator;
        }
        [HttpGet("groups/{symbols}")]
        public async Task<ActionResult<GroupsListStartWithVm>> GetGroupsStartingWith(string symbols)
        {
            var query = new GetGroupsListStartWith() { Symbols = symbols.ToUpper() };
            var vm = await _mediator.Send(query);
            return new JsonResult(vm){ StatusCode = Ok().StatusCode };
        }
        [HttpGet("{groupName}")]
        public async Task<ActionResult<GroupVm>> GetGroupTt(string groupName)
        {
            //if (groupName == "рома")
            //{
            //    return new JsonResult("ну рома же попущенка");
            //}
            //var groupTimetable = await Repo.GetGroup(groupName);
            //if (groupTimetable is null) return NoContent();
            //var jsonResult = new JsonResult(groupTimetable);
            //jsonResult.StatusCode = Ok().StatusCode;
            //return new JsonResult(groupTimetable);

            var query = new GetGroupWithTimetable() { Name = groupName.ToUpper() };
            var vm = await _mediator.Send(query);
            if (vm is null) return NoContent();
            return new JsonResult(vm) { StatusCode = Ok().StatusCode };
        }

    }
}
