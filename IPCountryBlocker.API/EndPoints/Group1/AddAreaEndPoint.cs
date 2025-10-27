//using FastEndpoints;
//using IPCountryBlocker.Application.DTOs;
//using IPCountryBlocker.Application.Resources;
//using IPCountryBlocker.Domain.Interfaces;
//using IPCountryBlocker.Domain.Models;
//using Microsoft.Extensions.Localization;

//namespace IPCountryBlocker.API.EndPoints.Group1
//{
//    public class AddAreaEndPoint : Endpoint<AddAreaRequest>
//    {
//        private readonly IAreaRepository _areaRepository;
//        private readonly IStringLocalizer<SharedResources> _localizer;

//        public AddAreaEndPoint(IAreaRepository areaRepository, IStringLocalizer<SharedResources> localizer)
//        {
//            _areaRepository = areaRepository;
//            _localizer = localizer;
//        }

//        public override void Configure()
//        {
//            Post("/area");
//            AllowAnonymous();
//            Summary(s =>
//            {
//                s.Summary = "Add a new area";
//                s.Description = "Adds a new area to the system.";
//                s.Responses[201] = "Area created successfully";
//                s.Responses[400] = "Bad request";
//            });
//            Options(x => x.WithTags("Area"));
//        }

//        public override async Task HandleAsync(AddAreaRequest req, CancellationToken ct)
//        {
//            var result = await _areaRepository.AddArea(new Area
//            {
//                Name = req.Name,
//                Zone = req.Zone
//            });

//            if (result)
//            {
//                await SendAsync(new { Message = _localizer[SharedResourcesKeys.Success] }, 201, ct);
//            }
//            else
//            {
//                await SendAsync(new { Message = _localizer[SharedResourcesKeys.AddFailed] }, 400, ct);
//            }
//        }

//    }
//}
