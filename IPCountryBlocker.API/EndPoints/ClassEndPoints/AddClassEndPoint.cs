using FastEndpoints;
using IPCountryBlocker.Application.DTOs.Class;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Application.Validators.Classes;
using IPCountryBlocker.Service.Abstractions;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.API.EndPoints.ClassEndPoints
{
    public class AddClassEndPoint : Endpoint<AddClassRequest>
    {
        private readonly IClassService _classService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddClassEndPoint(IClassService classService, IStringLocalizer<SharedResources> localizer)
        {
            _classService = classService;
            _localizer = localizer;
        }

        public override void Configure()
        {
            Post("/class");
            Validator<AddClassValidator>();
            AllowAnonymous();
            Options(x => x.WithTags("Class"));
        }

        public override async Task HandleAsync(AddClassRequest req, CancellationToken ct)
        {

            var result = await _classService.AddClassAsync(req.Id, req.Name, req.Teacher, req.Description);

            if (result)
            {
                await SendAsync(new { Message = _localizer[SharedResourcesKeys.Success] }, 201, ct);
            }
            else
            {
                await SendAsync(new { Message = _localizer[SharedResourcesKeys.AddFailed] }, 400, ct);
            }
        }
    }
}
