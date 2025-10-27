using FastEndpoints;
using IPCountryBlocker.Application.DTOs.Student;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Application.Validators.Students;
using IPCountryBlocker.Service.Abstractions;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.API.EndPoints.Student
{
    public class UpdateStudentEndPoint : Endpoint<UpdateStudentRequest>
    {
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UpdateStudentEndPoint(IStudentService studentService, IStringLocalizer<SharedResources> localizer)
        {
            _studentService = studentService;
            _localizer = localizer;
        }

        public override void Configure()
        {
            Put("/student/{id}");
            AllowAnonymous();
            Validator<UpdateStudentValidator>();
            Options(x => x.WithTags("Student"));
        }

        public override async Task HandleAsync(UpdateStudentRequest req, CancellationToken ct)
        {
            var routeId = Route<int>("id");

            if (routeId != req.Id)
            {
                await SendAsync(new { Message = "Route ID and body ID must match" }, 400, ct);
                return;
            }

            var result = await _studentService.UpdateStudentAsync(req.Id, req.FirstName, req.LastName, req.Age);

            if (result)
            {
                await SendAsync(new { Message = _localizer[SharedResourcesKeys.Success] }, 200, ct);
            }
            else
            {
                await SendAsync(new { Message = _localizer[SharedResourcesKeys.AddFailed] }, 400, ct);
            }
        }
    }
}
