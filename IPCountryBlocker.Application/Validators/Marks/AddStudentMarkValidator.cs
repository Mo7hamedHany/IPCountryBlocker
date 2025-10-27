using FluentValidation;
using IPCountryBlocker.Application.DTOs.Mark;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Domain.Interfaces;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Validators.Marks
{
    public class AddStudentMarkValidator : AbstractValidator<AddStudentMarkRequest>
    {
        private readonly IMarkRepository _markRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddStudentMarkValidator(
            IMarkRepository markRepository,
            IEnrollmentRepository enrollmentRepository,
            IStringLocalizer<SharedResources> localizer)
        {
            _markRepository = markRepository;
            _enrollmentRepository = enrollmentRepository;
            _localizer = localizer;
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(0)
                .WithMessage("Mark ID must be greater than 0");

            RuleFor(x => x.StudentId)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(0)
                .WithMessage("Student ID must be greater than 0");

            RuleFor(x => x.ClassId)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(0)
                .WithMessage("Class ID must be greater than 0");

            RuleFor(x => x.ExamMark)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .GreaterThanOrEqualTo(0)
                .WithMessage("Exam mark must be greater than or equal to 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Exam mark cannot exceed 100");

            RuleFor(x => x.AssignmentMark)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .GreaterThanOrEqualTo(0)
                .WithMessage("Assignment mark must be greater than or equal to 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Assignment mark cannot exceed 100");

            RuleFor(x => x)
                .MustAsync(StudentIsEnrolledInClassAsync)
                .WithMessage("Student must be enrolled in the class before marks can be recorded")
                .When(x => x.StudentId > 0 && x.ClassId > 0);
        }

        private async Task<bool> StudentIsEnrolledInClassAsync(AddStudentMarkRequest request, CancellationToken cancellationToken)
        {
            var isEnrolled = await _enrollmentRepository.IsAlreadyEnrolled(request.StudentId, request.ClassId);
            return isEnrolled;
        }
    }
}
