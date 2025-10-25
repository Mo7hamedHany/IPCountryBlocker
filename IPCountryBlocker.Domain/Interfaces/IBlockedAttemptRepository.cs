using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Domain.Interfaces
{
    public interface IBlockedAttemptRepository
    {
        Task<bool> AddBlockedAttemptAsync(BlockedAttempt attempt);
        Task<PagedResult<BlockedAttempt>> GetBlockedAttemptsAsync(int pageNumber = 1, int pageSize = 10);
    }
}
