using FastEndpoints;
using IPCountryBlocker.Application.DTOs.Mark;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Service.Abstractions;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.API.EndPoints.ClassEndPoints
{
    public class GetClassStudentsAverageMarkEndPoint : EndpointWithoutRequest<AverageMarkResponse>
    {
        private readonly IMarkService _markService;
        private readonly IClassService _classService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetClassStudentsAverageMarkEndPoint(
            IMarkService markService,
            IClassService classService,
            IStringLocalizer<SharedResources> localizer)
        {
            _markService = markService;
            _classService = classService;
            _localizer = localizer;
        }

        public override void Configure()
        {
            Get("/api/classes/{classId}/average-mark");
            AllowAnonymous();
            Options(x => x.WithTags("Class"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            if (!int.TryParse(Route<string>("classId"), out var classId) || classId <= 0)
            {
                ThrowError("Invalid class ID");
            }

            var classExists = await _classService.GetClassByIdAsync(classId);
            if (classExists == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var averageMark = await _markService.GetAverageClassStudentsMarkAsync(classId);

            var response = new AverageMarkResponse
            {
                Message = averageMark == 0
                ? "No marks recorded for students in this class"
                : _localizer[SharedResourcesKeys.Success],
                AverageMark = Math.Round(averageMark, 2)
            };

            await SendOkAsync(response, ct);
        }
    }
}
