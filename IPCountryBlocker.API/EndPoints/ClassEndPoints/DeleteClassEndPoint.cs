using FastEndpoints;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Service.Abstractions;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.API.EndPoints.ClassEndPoints
{
    public class DeleteClassEndPoint : EndpointWithoutRequest
    {
        private readonly IClassService _classService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DeleteClassEndPoint(IClassService classService, IStringLocalizer<SharedResources> localizer)
        {
            _classService = classService;
            _localizer = localizer;
        }

        public override void Configure()
        {
            Delete("/class/{id}");
            AllowAnonymous();
            Options(x => x.WithTags("Class"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");

            var result = await _classService.DeleteClassAsync(id);

            if (!result)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendAsync(new { Message = _localizer[SharedResourcesKeys.Success] }, 200, ct);
        }
    }
}
