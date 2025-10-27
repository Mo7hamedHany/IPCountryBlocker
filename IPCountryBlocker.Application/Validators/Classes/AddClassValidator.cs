using FluentValidation;
using IPCountryBlocker.Application.DTOs.Class;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Domain.Interfaces;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Validators.Classes
{
    public class AddClassValidator : AbstractValidator<AddClassRequest>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IClassRepository _classRepository;

        public AddClassValidator(IStringLocalizer<SharedResources> localizer, IClassRepository classRepository)
        {
            _localizer = localizer;
            _classRepository = classRepository;
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
                .WithMessage("Class ID must be greater than 0")
                .MustAsync(ClassIdDoesNotExistAsync)
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100])
                .MinimumLength(2)
                .WithMessage("Class name must be at least 2 characters");

            RuleFor(x => x.Teacher)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100])
                .MinimumLength(2)
                .WithMessage("Teacher name must be at least 2 characters");

            RuleFor(x => x.Description)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters")
                .MinimumLength(10)
                .WithMessage("Description must be at least 10 characters");
        }

        private async Task<bool> ClassIdDoesNotExistAsync(int classId, CancellationToken cancellationToken)
        {
            var classObj = await _classRepository.GetClassById(classId);
            return classObj == null;
        }
    }
}
