using FluentValidation;
using IPCountryBlocker.Application.Features.Countries.Commands.Models;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Domain.Interfaces;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Features.Countries.Commands.Validators
{
    public class RemoveCountryFromBlockedListValidator : AbstractValidator<RemoveCountryFromBlockedListCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICountryRepository _countryRepository;

        public RemoveCountryFromBlockedListValidator(IStringLocalizer<SharedResources> localizer, ICountryRepository countryRepository)
        {
            _localizer = localizer;
            _countryRepository = countryRepository;
            ApplyValidationRules();
        }
        public void ApplyValidationRules()
        {
            RuleFor(x => x.Code)
               .NotNull()
               .WithMessage(_localizer[SharedResourcesKeys.Required])
               .NotEmpty()
               .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
               .Matches(@"^[A-Z]{2}$")
               .WithMessage(_localizer[SharedResourcesKeys.AlphaFormat])
               .MustAsync(BeValidCountryCode)
               .WithMessage(_localizer[SharedResourcesKeys.InvaildCode]);
            RuleFor(x => x.Code)
                .MustAsync(CountryCodeExistsAsync)
                .WithMessage(_localizer[SharedResourcesKeys.NotFound])
                .When(x => !string.IsNullOrWhiteSpace(x.Code));
        }

        private Task<bool> BeValidCountryCode(string? countryCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                return Task.FromResult(false);

            try
            {

                var region = new System.Globalization.RegionInfo(countryCode.ToUpper());

                var isValid = !string.IsNullOrWhiteSpace(region.TwoLetterISORegionName)
                              && region.TwoLetterISORegionName.Equals(countryCode, StringComparison.OrdinalIgnoreCase);

                return Task.FromResult(isValid);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        private async Task<bool> CountryCodeExistsAsync(string? countryCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                return false;
            var country = await _countryRepository.GetBlockedCountryAsync(countryCode!);
            return country != null;
        }
    }
}
