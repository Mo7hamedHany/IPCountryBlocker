using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace IPCountryBlocker.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDatabase _database;

        public CountryRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<bool> AddBlockedCountryAsync(Country country, int? blockDuration)
        {
            var countryJson = JsonConvert.SerializeObject(country);

            if (blockDuration.HasValue)
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(blockDuration.Value);
                var temporal = new TemporalBlock
                {
                    Country = country,
                    ExpiresAt = expiresAt
                };
                var temporalJson = JsonConvert.SerializeObject(temporal);
                return await _database.HashSetAsync("BlockedCountries:Temporary", country.Code, temporalJson);
            }
            else
            {
                return await _database.HashSetAsync("BlockedCountries:Permanent", country.Code, countryJson);
            }
        }

        public async Task<PagedResult<Country>> GetBlockedCountriesAsync(string? searchItem, int pageNumber = 1, int pageSize = 10)
        {

            var countries = new List<Country>();

            var permanentEntries = await _database.HashGetAllAsync("BlockedCountries:Permanent");
            foreach (var entry in permanentEntries)
            {
                if (entry.Value.IsNullOrEmpty) continue;

                var country = JsonConvert.DeserializeObject<Country>(entry.Value);
                if (country != null)
                {
                    country.IsTemporary = false;
                    countries.Add(country);
                }
            }

            var temporaryEntries = await _database.HashGetAllAsync("BlockedCountries:Temporary");
            foreach (var entry in temporaryEntries)
            {
                if (entry.Value.IsNullOrEmpty) continue;

                var temporalBlock = JsonConvert.DeserializeObject<TemporalBlock>(entry.Value);
                var country = temporalBlock.Country;
                if (country != null)
                {
                    country.IsTemporary = true;
                    countries.Add(country);
                }
            }

            if (!string.IsNullOrWhiteSpace(searchItem))
            {
                countries = countries.Where(c =>
                    (c.Code != null && c.Code.Contains(searchItem, StringComparison.OrdinalIgnoreCase)) ||
                    (c.Name != null && c.Name.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            var totalCount = countries.Count;

            var pagedCountries = countries
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Country>
            {
                Items = pagedCountries,
                TotalCount = totalCount
            };

        }

        public async Task<Country?> GetBlockedCountryAsync(string countryCode)
        {

            var permanentValue = await _database.HashGetAsync("BlockedCountries:Permanent", countryCode);
            if (!permanentValue.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<Country>(permanentValue!);
            }
            var temporaryValue = await _database.HashGetAsync("BlockedCountries:Temporary", countryCode);
            if (!temporaryValue.IsNullOrEmpty)
            {
                return (JsonConvert.DeserializeObject<TemporalBlock>(temporaryValue!)!).Country;

            }

            var countryJson = await _database.StringGetAsync(countryCode);
            if (countryJson.IsNullOrEmpty)
            {
                return null;
            }
            var country = JsonConvert.DeserializeObject<Country>(countryJson);
            return country;
        }

        public async Task<List<TemporalBlock>> GetExpiredTemporaryBlocksAsync()
        {
            var expiredBlocks = new List<TemporalBlock>();


            var entries = await _database.HashGetAllAsync("BlockedCountries:Temporary");
            if (entries == null || entries.Length == 0)
                return expiredBlocks;

            foreach (var entry in entries)
            {
                var blockJson = entry.Value;
                if (blockJson.IsNullOrEmpty)
                    continue;


                var block = JsonConvert.DeserializeObject<TemporalBlock>(blockJson);


                if (block == null)
                    continue;

                if (block.ExpiresAt <= DateTime.UtcNow)
                {
                    expiredBlocks.Add(block);
                }
            }

            return expiredBlocks;
        }

        public async Task<bool> RemoveBlockedCountryAsync(string countryCode)
        {
            var permanentDeleted = await _database.HashDeleteAsync("BlockedCountries:Permanent", countryCode);

            var temporaryDeleted = await _database.HashDeleteAsync("BlockedCountries:Temporary", countryCode);

            return permanentDeleted || temporaryDeleted;
        }
    }
}
