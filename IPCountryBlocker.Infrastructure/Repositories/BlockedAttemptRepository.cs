using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace IPCountryBlocker.Infrastructure.Repositories
{
    public class BlockedAttemptRepository : IBlockedAttemptRepository
    {
        private readonly IDatabase _database;

        public BlockedAttemptRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<bool> AddBlockedAttemptAsync(BlockedAttempt attempt)
        {
            attempt.CountryCode = attempt.CountryCode.ToUpper();
            attempt.Timestamp = DateTime.UtcNow;

            var attemptJson = JsonConvert.SerializeObject(attempt);

            var result = await _database.StringSetAsync($"blocked-attempt::{attempt.Id}", attemptJson);

            return result;
        }

        public async Task<PagedResult<BlockedAttempt>> GetBlockedAttemptsAsync(int pageNumber = 1, int pageSize = 10)
        {
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
            var allKeys = server.Keys(pattern: "blocked-attempt::*").ToList();
            var attempts = new List<BlockedAttempt>();

            foreach (var key in allKeys)
            {
                var blockedAttempt = await _database.StringGetAsync(key);
                if (!blockedAttempt.IsNullOrEmpty)
                {
                    var attempt = JsonConvert.DeserializeObject<BlockedAttempt>(blockedAttempt);
                    if (attempt != null)
                    {
                        attempts.Add(attempt);
                    }
                }
            }
            var totalCount = attempts.Count;
            var pagedAttempts = attempts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<BlockedAttempt>()
            {

                Items = pagedAttempts,
                TotalCount = totalCount
            };

        }
    }
}
