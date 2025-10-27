using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace IPCountryBlocker.Infrastructure.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly IDatabase _database;
        private const string AreaKeyPrefix = "area:";

        public AreaRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<bool> AddArea(Area area)
        {
            var areaJson = JsonSerializer.Serialize(area);
            // Store with prefix to distinguish from other keys
            return await _database.StringSetAsync($"{AreaKeyPrefix}{area.Name}", areaJson);
        }

        public async Task<List<Area>> GetAllAreasAsync()
        {
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());

            // Only get keys that start with "area:"
            var areaKeys = server.Keys(pattern: $"{AreaKeyPrefix}*").ToArray();

            var areas = new List<Area>();

            foreach (var key in areaKeys)
            {
                var areaJson = await _database.StringGetAsync(key);
                if (!areaJson.IsNullOrEmpty)
                {
                    var area = JsonSerializer.Deserialize<Area>(areaJson);
                    if (area != null)
                    {
                        areas.Add(area);
                    }
                }
            }

            return areas;
        }

        public async Task<Area?> GetAreaByNameAsync(string areaName)
        {
            // Use the prefix when looking up
            var areaJson = await _database.StringGetAsync($"{AreaKeyPrefix}{areaName}");

            if (areaJson.IsNullOrEmpty)
                return null;

            var area = JsonSerializer.Deserialize<Area>(areaJson);

            return area;
        }
    }
}
