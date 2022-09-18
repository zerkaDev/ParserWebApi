using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timetable.Application.Interfaces;
using Timetable.Domain;

namespace Timetable.WepApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimetableController : ControllerBase
    {
        ITimetableRepository Repo { get; }
        public TimetableController(ITimetableRepository repo, ITimetableDbContext context)
        {
            Repo = repo;
        }
        [HttpGet("{groupName}")]
        public async Task<ActionResult<Group>> Get(string groupName)
        {
            if (groupName == "рома") 
            {
                return new JsonResult("ну рома же попущенка");
            }
            var groupTimetable = await Repo.GetGroup(groupName);
            if (groupTimetable is null) return NoContent();
            var jsonResult = new JsonResult(groupTimetable);
            jsonResult.StatusCode = Ok().StatusCode;
            return new JsonResult(groupTimetable);
        }

    }
}
