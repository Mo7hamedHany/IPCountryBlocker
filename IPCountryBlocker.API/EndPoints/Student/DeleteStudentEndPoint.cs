using FastEndpoints;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Service.Abstractions;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.API.EndPoints.Student
{
    public class DeleteStudentEndPoint : EndpointWithoutRequest
    {
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DeleteStudentEndPoint(IStudentService studentService, IStringLocalizer<SharedResources> localizer)
        {
            _studentService = studentService;
            _localizer = localizer;
        }

        public override void Configure()
        {
            Delete("/student/{id}");
            AllowAnonymous();
            Options(x => x.WithTags("Student"));

        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");

            var result = await _studentService.DeleteStudentAsync(id);

            if (!result)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendAsync(new { Message = _localizer[SharedResourcesKeys.Success] }, 200, ct);
        }
    }
}
