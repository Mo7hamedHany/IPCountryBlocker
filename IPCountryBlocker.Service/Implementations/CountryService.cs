using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;

namespace IPCountryBlocker.Service.Implementations
{
    public class CountryService : ICountryService
    {

        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<string> AddBlockedCountryAsync(string countryCode, string countryName, int? blockDuration)
        {
            var country = await _countryRepository.GetBlockedCountryAsync(countryCode);

            if (country is not null)
                return "Exists";

            country = new Country
            {
                Code = countryCode,
                Name = countryName,
                IsBlocked = true,
                IsTemporary = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var result = await _countryRepository.AddBlockedCountryAsync(country, blockDuration);

            return result ? "Success" : "Failed";
        }

        public async Task<PagedResult<Country>> GetBlockedCountriesAsync(string? searchItem, int pageNumber = 1, int pageSize = 10)
            => await _countryRepository.GetBlockedCountriesAsync(searchItem, pageNumber, pageSize);

        public async Task<string> RemoveBlockedCountryAsync(string countryCode)
        {
            var country = await _countryRepository.GetBlockedCountryAsync(countryCode.ToUpper());

            if (country is null)
                return "NotFound";

            var result = await _countryRepository.RemoveBlockedCountryAsync(country.Code);

            return result ? "Success" : "Failed";
        }
    }
}
