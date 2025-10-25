using IPCountryBlocker.API.Bases;
using IPCountryBlocker.Application.Features.Logging.Query.Models;
using Microsoft.AspNetCore.Mvc;

namespace IPCountryBlocker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : AppControllerBase
    {
        [HttpGet("blocked-attempts")]
        public async Task<ActionResult> GetBlockedAttempts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
            => NewResult(await Mediator.Send(new GetBlockedAttemptsQuery(pageNumber, pageSize)));

    }
}
