using FastEndpoints;
using IPCountryBlocker.Application.DTOs.Mark;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Application.Validators.Marks;
using IPCountryBlocker.Service.Abstractions;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.API.EndPoints.Mark
{
    public class AddStudentMarkEndPoint : Endpoint<AddStudentMarkRequest>
    {
        private readonly IMarkService _markService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddStudentMarkEndPoint(IMarkService markService, IStringLocalizer<SharedResources> localizer)
        {
            _markService = markService;
            _localizer = localizer;
        }

        public override void Configure()
        {
            Post("/mark");
            AllowAnonymous();
            Validator<AddStudentMarkValidator>();
            Options(x => x.WithTags("Mark"));
        }

        public override async Task HandleAsync(AddStudentMarkRequest req, CancellationToken ct)
        {
            var result = await _markService.AddStudentMarkAsync(
                req.Id,
                req.StudentId,
                req.ClassId,
                (int)req.ExamMark,
                (int)req.AssignmentMark
            );

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
