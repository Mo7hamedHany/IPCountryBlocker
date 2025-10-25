using IPCountryBlocker.Domain.Interfaces;

namespace IPCountryBlocker.Service.Implementations
{
    public class TemporaryBlockCleaner
    {
        private readonly ICountryRepository _countryRepository;

        public TemporaryBlockCleaner(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task RemoveExpiredBlocksAsync()
        {
            var expiredCountries = await _countryRepository.GetExpiredTemporaryBlocksAsync();
            foreach (var temporalBlock in expiredCountries)
            {
                await _countryRepository.RemoveBlockedCountryAsync(temporalBlock.Country.Code);
            }
        }
    }
}
