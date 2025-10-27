using FluentValidation;
using IPCountryBlocker.Application.DTOs.Student;

namespace IPCountryBlocker.Application.Validators.Students
{
    public class GetStudentsValidator : AbstractValidator<GetStudentsRequest>
    {
        public GetStudentsValidator()
        {
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be at least 1");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page size must be at least 1")
                .LessThanOrEqualTo(100)
                .WithMessage("Page size cannot exceed 100");

            RuleFor(x => x.SearchItem)
                .MaximumLength(200)
                .WithMessage("Search item cannot exceed 200 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.SearchItem));
        }
    }
}