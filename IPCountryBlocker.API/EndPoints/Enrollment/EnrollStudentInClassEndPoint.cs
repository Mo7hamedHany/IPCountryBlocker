using FastEndpoints;
using IPCountryBlocker.Application.DTOs.Enrollment;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Application.Validators.Enrollments;
using IPCountryBlocker.Service.Abstractions;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.API.EndPoints.Enrollment
{
    public class EnrollStudentInClassEndPoint : Endpoint<EnrollStudentInClassRequest>
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public EnrollStudentInClassEndPoint(IEnrollmentService enrollmentService, IStringLocalizer<SharedResources> localizer)
        {
            _enrollmentService = enrollmentService;
            _localizer = localizer;
        }

        public override void Configure()
        {
            Post("/enrollment");
            AllowAnonymous();
            Validator<EnrollStudentInClassValidator>();
            Options(x => x.WithTags("Enrollment"));
        }

        public override async Task HandleAsync(EnrollStudentInClassRequest req, CancellationToken ct)
        {
            var result = await _enrollmentService.EnrollStudentAsync(req.Id, req.StudentId, req.ClassId);

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
