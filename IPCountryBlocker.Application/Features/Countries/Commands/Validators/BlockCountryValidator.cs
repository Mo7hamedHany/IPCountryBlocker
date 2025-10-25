using FluentValidation;
using IPCountryBlocker.Application.Features.Countries.Commands.Models;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Domain.Interfaces;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Features.Countries.Commands.Validators
{
    public class BlockCountryValidator : AbstractValidator<BlockCountryCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICountryRepository _countryRepository;

        public BlockCountryValidator(IStringLocalizer<SharedResources> localizer, ICountryRepository countryRepository)
        {
            _localizer = localizer;
            ApplyValidationRules();
            _countryRepository = countryRepository;
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

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100)
                .WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100])
                .MinimumLength(2)
                .WithMessage(_localizer[SharedResourcesKeys.InvaildCode]);

            RuleFor(x => x.Code)
                .MustAsync(CountryCodeDoesNotExistAsync)
                .WithMessage(_localizer[SharedResourcesKeys.IsExist])
                .When(x => !string.IsNullOrWhiteSpace(x.Code));
        }


        //private Task<bool> BeValidCountryCode(string? countryCode, CancellationToken cancellationToken)
        //{
        //    if (string.IsNullOrWhiteSpace(countryCode))
        //        return Task.FromResult(false);

        //    var isValid = Regex.IsMatch(countryCode.ToUpper(), @"^[A-Z]{2}$");
        //    return Task.FromResult(isValid);
        //}

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

        private async Task<bool> CountryCodeDoesNotExistAsync(string? countryCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                return false;

            var existingCountry = await _countryRepository.GetBlockedCountryAsync(countryCode.ToUpper());
            return existingCountry == null;

        }
    }
}
