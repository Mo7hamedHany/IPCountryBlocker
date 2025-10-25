using IPCountryBlocker.API.Bases;
using IPCountryBlocker.Application.Features.Ip.Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace IPCountryBlocker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : AppControllerBase
    {
        [HttpGet("lookup")]
        public async Task<ActionResult> GetIpLookup([FromQuery] string? ipAddress)
        {
            var query = new GetIpLookupQuery(ipAddress, HttpContext);
            var ippAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            return NewResult(await Mediator.Send(query));
        }

        [HttpGet("check-block")]
        public async Task<ActionResult> CheckIfBlocked()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            return NewResult(await Mediator.Send(new CheckIfBlockedQuery(userAgent)));
        }

    }
}
