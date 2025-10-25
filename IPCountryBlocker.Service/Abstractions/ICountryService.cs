using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Service.Abstractions
{
    public interface ICountryService
    {
        Task<string> AddBlockedCountryAsync(string countryCode, string countryName, int? blockDuration);
        Task<string> RemoveBlockedCountryAsync(string countryCode);
        Task<PagedResult<Country>> GetBlockedCountriesAsync(string? searchItem, int pageNumber = 1, int pageSize = 10);

    }
}
