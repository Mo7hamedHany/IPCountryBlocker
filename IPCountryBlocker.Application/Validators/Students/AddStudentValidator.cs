using FluentValidation;
using IPCountryBlocker.Application.DTOs.Student;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Domain.Interfaces;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Validators.Students
{
    public class AddStudentValidator : AbstractValidator<AddStudentRequest>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IStudentRepository _studentRepository;

        public AddStudentValidator(IStringLocalizer<SharedResources> localizer, IStudentRepository studentRepository)
        {
            _localizer = localizer;
            _studentRepository = studentRepository;
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
                .WithMessage("Student ID must be greater than 0")
                .MustAsync(StudentIdDoesNotExistAsync)
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

            RuleFor(x => x.FirstName)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100])
                .MinimumLength(2)
                .WithMessage("First name must be at least 2 characters");

            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100])
                .MinimumLength(2)
                .WithMessage("Last name must be at least 2 characters");

            RuleFor(x => x.Age)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThan(5)
                .WithMessage("Age must be greater than 5")
                .LessThanOrEqualTo(120)
                .WithMessage("Age must be less than or equal to 120");
        }

        private async Task<bool> StudentIdDoesNotExistAsync(int studentId, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetStudentById(studentId);
            return student == null;
        }
    }
}
