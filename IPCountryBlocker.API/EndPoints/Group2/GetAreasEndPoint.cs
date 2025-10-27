//using FastEndpoints;
//using IPCountryBlocker.Application.Resources;
//using IPCountryBlocker.Domain.Interfaces;
//using IPCountryBlocker.Domain.Models;
//using Microsoft.Extensions.Localization;

//namespace IPCountryBlocker.API.EndPoints.Group2
//{
//    public class GetAreasEndPoint : EndpointWithoutRequest<List<Area>>
//    {
//        private readonly IAreaRepository _areaRepository;
//        private readonly IStringLocalizer<SharedResources> _localizer;

//        public GetAreasEndPoint(IAreaRepository areaRepository, IStringLocalizer<SharedResources> localizer)
//        {
//            _areaRepository = areaRepository;
//            _localizer = localizer;
//        }

//        public override void Configure()
//        {
//            Get("/area");
//            AllowAnonymous();
//            Options(x => x.WithTags("Area"));
//        }

//        public override async Task HandleAsync(CancellationToken ct)
//        {
//            var areas = await _areaRepository.GetAllAreasAsync();
//            if (areas == null || !areas.Any())
//            {
//                await SendNotFoundAsync(ct);
//                return;
//            }

//            await SendOkAsync(areas.ToList(), ct);
//        }
//    }
//}
