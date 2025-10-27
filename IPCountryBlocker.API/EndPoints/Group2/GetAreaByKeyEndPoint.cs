//using FastEndpoints;
//using IPCountryBlocker.Application.Resources;
//using IPCountryBlocker.Domain.Interfaces;
//using IPCountryBlocker.Domain.Models;
//using Microsoft.Extensions.Localization;

//namespace IPCountryBlocker.API.EndPoints.Group2
//{
//    public class GetAreaByKeyEndPoint : EndpointWithoutRequest<Area>
//    {
//        private readonly IAreaRepository _areaRepository;
//        private readonly IStringLocalizer<SharedResources> _localizer;

//        public GetAreaByKeyEndPoint(IAreaRepository areaRepository, IStringLocalizer<SharedResources> localizer)
//        {
//            _areaRepository = areaRepository;
//            _localizer = localizer;
//        }

//        public override void Configure()
//        {
//            Get("/area/{key}");
//            AllowAnonymous();
//            Options(x => x.WithTags("Area"));

//            Summary(s =>
//            {
//                s.Summary = "Get area by key";
//                s.Description = "Retrieves an area by its name/key";
//                s.Params["key"] = "The area name/key to search for";
//                s.Responses[200] = "Area found";
//                s.Responses[404] = "Area not found";
//            });
//        }

//        public override async Task HandleAsync(CancellationToken ct)
//        {
//            // Get the key from route values
//            var key = Route<string>("key");

//            var area = await _areaRepository.GetAreaByNameAsync(key);

//            if (area == null)
//            {
//                await SendNotFoundAsync(ct);
//                return;
//            }

//            await SendOkAsync(area, ct);
//        }
//    }
//}
