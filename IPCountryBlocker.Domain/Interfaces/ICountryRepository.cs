using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Domain.Interfaces
{
    public interface ICountryRepository
    {
        Task<bool> AddBlockedCountryAsync(Country country, int? blockDuration);
        Task<Country?> GetBlockedCountryAsync(string countryCode);
        Task<bool> RemoveBlockedCountryAsync(string countryCode);
        Task<PagedResult<Country>> GetBlockedCountriesAsync(string? searchItem = null, int pageNumber = 1, int pageSize = 10);
        Task<List<TemporalBlock>> GetExpiredTemporaryBlocksAsync();
    }
}
