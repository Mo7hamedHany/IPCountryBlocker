using FastEndpoints;
using IPCountryBlocker.Application.DTOs.Class;
using IPCountryBlocker.Application.Validators.Classes;
using IPCountryBlocker.Service.Abstractions;

namespace IPCountryBlocker.API.EndPoints.ClassEndPoints
{
    public class GetClassesEndPoint : Endpoint<GetClassesRequest, ClassesPagedResponse>
    {
        private readonly IClassService _classService;

        public GetClassesEndPoint(IClassService classService)
        {
            _classService = classService;
        }

        public override void Configure()
        {
            Get("/classes");
            AllowAnonymous();
            Validator<GetClassesValidator>();
            Options(x => x.WithTags("Class"));
        }

        public override async Task HandleAsync(GetClassesRequest req, CancellationToken ct)
        {
            var result = await _classService.GetAllClassAsync(req.SearchItem, req.PageNumber, req.PageSize);

            if (result == null || result.Items == null || !result.Items.Any())
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var response = new ClassesPagedResponse
            {
                Classes = result.Items,
                TotalCount = result.TotalCount,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize,
                TotalPages = (int)Math.Ceiling(result.TotalCount / (double)req.PageSize)
            };

            await SendOkAsync(response, ct);
        }
    }
}
