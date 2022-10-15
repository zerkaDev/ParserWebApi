using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Timetable.Application.Groups.Queries.GetGroupList;
using Timetable.Application.Queries.Groups;
using Timetable.Application.Teachers.Queries.GetTeacherList;
using Timetable.Application.Teachers.Queries.GetTeachersFromGroup;
using Timetable.Application.Teachers.Queries.GetTeacherTt;

namespace Timetable.WepApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class TimetableController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TimetableController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Gets groups id's starting with the entered symbols
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/timetable/groups/22-К
        /// </remarks>
        /// <param name="symbols">Symbols</param>
        /// <returns>List groups id's</returns>
        [HttpGet("groups/{symbols}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<JsonResult> GetGroupsStartingWith(string symbols)
        {
            var query = new GetGroupsListStartWith() { Symbols = symbols.ToUpper() };
            var vm = await _mediator.Send(query);
            return new JsonResult(vm) { StatusCode = Ok().StatusCode };
        }
        /// <summary>
        /// Gets teachers names starting with the entered symbols
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/timetable/teachers/Ив
        /// </remarks>
        /// <param name="symbols">Symbols</param>
        /// <returns>List teachers names</returns>
        [HttpGet("teachers/{symbols}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<JsonResult> GetTeachersStartingWith(string symbols)
        {
            var query = new GetTeacherListStartsWith() { Symbols = symbols };
            var vm = await _mediator.Send(query);
            return new JsonResult(vm) { StatusCode = Ok().StatusCode };
        }
        /// <summary>
        /// Gets the teacher's timetable by his full name
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/timetable/teachertt/Иванов Иван Иванович
        /// Attention! All words must begin with a capital letter and there must be a space between words
        /// </remarks>
        /// <param name="fio">Teacher's full name</param>
        /// <returns>Teacher timetable view model with weeks, days and lessons</returns>
        /// <response code="200">Success</response>
        /// <response code="204">Teacher not found</response>
        [HttpGet("teachertt/{fio}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<TeacherTtVm>> GetTeacherTimetable(string fio)
        {
            var query = new GetTeacherTt() { TeacherFullName = fio };
            var vm = await _mediator.Send(query);
            if (vm is null) return NoContent();
            return new JsonResult(vm) { StatusCode = Ok().StatusCode};
        }
        /// <summary>
        /// Gets all teachers teaching in the given group
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/timetable/teachersOfGroup/20-КБ-ПИ2
        /// </remarks>
        /// <param name="groupName">Unique group id</param>
        /// <returns>List teachers name</returns>
        /// <response code="200">Success</response>
        /// <response code="204">Teacher not found</response>
        [HttpGet("teachersOfGroup/{groupName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<TeachersFromGroupVm>> GetTeacherOfGroup(string groupName)
        {
            var query = new GetTeachersFromGroup() { GroupName = groupName };
            var vm = await _mediator.Send(query);
            if (vm is null) return NoContent();
            return new JsonResult(vm) { StatusCode = Ok().StatusCode };
        }
        /// <summary>
        /// Gets the group with timetable by group name
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/timetable/kubstu/20-КБ-ПИ2
        /// </remarks>
        /// <param name="groupName">Unique group id</param>
        /// <param name="university">Unique university name</param>
        /// <returns>Group view model, contains weeks, which contains days, which contains lessons</returns>
        /// <response code="200">Success</response>
        /// <response code="204">Group not found</response>
        [HttpGet("{university}/{groupName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<GroupVm>> GetGroupTt(string university, string groupName)
        {
            var query = new GetGroupWithTimetable() { University = university, Name = groupName.ToUpper() };
            var vm = await _mediator.Send(query);
            if (vm is null) return NoContent();
            return new JsonResult(vm) { StatusCode = Ok().StatusCode };
        }

    }
}
