using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace IPCountryBlocker.Infrastructure.Repositories
{
    public class ClassRepository : IClassRepository
    {

        private readonly IDatabase _database;
        private readonly IServer _server;
        private const string ClassKeyPrefix = "class:";

        public ClassRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
            _server = connection.GetServer(connection.GetEndPoints().First());
        }

        public async Task<bool> AddClassAsync(Class obj)
        {
            var classJson = System.Text.Json.JsonSerializer.Serialize(obj);

            var result = await _database.StringSetAsync($"{ClassKeyPrefix}{obj.Id}", classJson);

            return result;
        }

        public async Task<bool> DeleteClassAsync(int classId)
        {
            var deleted = await _database.KeyDeleteAsync($"{ClassKeyPrefix}{classId}");

            return deleted;
        }

        public async Task<PagedResult<Class>> GetClassesAsync(string? searchItem = null, int pageNumber = 1, int pageSize = 10)
        {
            var classes = new List<Class>();

            var keys = _server.Keys(pattern: $"{ClassKeyPrefix}*").ToList();

            foreach (var key in keys)
            {
                var classJson = await _database.StringGetAsync(key);

                if (classJson.IsNullOrEmpty) continue;

                var @class = JsonSerializer.Deserialize<Class>(classJson);
                if (@class != null)
                {
                    classes.Add(@class);
                }
            }


            if (!string.IsNullOrWhiteSpace(searchItem))
            {
                classes = classes.Where(s =>
                    (s.Name != null && s.Name.Contains(searchItem, StringComparison.OrdinalIgnoreCase)) ||
                    (s.Teacher != null && s.Teacher.Contains(searchItem, StringComparison.OrdinalIgnoreCase)) ||
                    s.Id.ToString().Contains(searchItem)
                ).ToList();
            }


            var totalCount = classes.Count;


            var pagedClasses = classes
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Class>
            {
                Items = pagedClasses,
                TotalCount = totalCount
            };
        }

        public async Task<Class?> GetClassById(int classId)
        {
            var classJson = await _database.StringGetAsync($"{ClassKeyPrefix}{classId}");

            if (classJson.IsNullOrEmpty)
                return null;

            var classObj = JsonSerializer.Deserialize<Class>(classJson);

            return classObj;
        }
    }
}
