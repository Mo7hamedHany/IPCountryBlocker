using IPCountryBlocker.Domain.Models;

namespace IPCountryBlocker.Service.Abstractions
{
    public interface ILoggingService
    {
        Task<PagedResult<BlockedAttempt>> GetBlockedAttemptsAsync(int pageNumber = 1, int pageSize = 10);
    }
}
