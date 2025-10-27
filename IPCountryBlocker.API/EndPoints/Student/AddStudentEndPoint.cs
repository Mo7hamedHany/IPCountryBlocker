using FastEndpoints;
using IPCountryBlocker.Application.DTOs.Student;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Application.Validators.Students;
using IPCountryBlocker.Service.Abstractions;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.API.EndPoints.Student
{
    public class AddStudentEndPoint : Endpoint<AddStudentRequest>
    {
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddStudentEndPoint(IStudentService studentService, IStringLocalizer<SharedResources> localizer)
        {
            _studentService = studentService;
            _localizer = localizer;
        }

        public override void Configure()
        {
            Post("/student");
            Validator<AddStudentValidator>();
            AllowAnonymous();
            Options(x => x.WithTags("Student"));
        }

        public override async Task HandleAsync(AddStudentRequest req, CancellationToken ct)
        {
            var result = await _studentService.AddStudentAsync(req.Id, req.FirstName, req.LastName, req.Age);

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
