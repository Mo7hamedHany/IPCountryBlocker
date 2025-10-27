//using IPCountryBlocker.API.Bases;
//using IPCountryBlocker.Application.Features.Countries.Commands.Models;
//using IPCountryBlocker.Application.Features.Countries.Queries.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace IPCountryBlocker.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CountryController : AppControllerBase
//    {
//        [HttpPost("block")]
//        public async Task<ActionResult> BlockCountry([FromBody] BlockCountryCommand command)
//            => NewResult(await Mediator.Send(command));

//        [HttpPost("temporal-block")]
//        public async Task<ActionResult> BlockCountryTemporary([FromBody] TemporalCountryBlockCommand command)
//           => NewResult(await Mediator.Send(command));

//        [HttpDelete("unblock/{code}")]
//        public async Task<ActionResult> UnblockCountry([FromRoute] string code)
//            => NewResult(await Mediator.Send(new RemoveCountryFromBlockedListCommand(code)));

//        [HttpGet("blocked")]
//        public async Task<ActionResult> GetBlockedCountries([FromQuery] string? searchItem, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
//            => NewResult(await Mediator.Send(new GetAllBlockedCountriesQuery(searchItem, pageNumber, pageSize)));
//    }
//}
