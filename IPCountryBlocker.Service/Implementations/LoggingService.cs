using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;

namespace IPCountryBlocker.Service.Implementations
{
    public class LoggingService : ILoggingService
    {
        private readonly IBlockedAttemptRepository _blockedAttemptRepository;

        public LoggingService(IBlockedAttemptRepository blockedAttemptRepository)
        {
            _blockedAttemptRepository = blockedAttemptRepository;
        }

        public async Task<PagedResult<BlockedAttempt>> GetBlockedAttemptsAsync(int pageNumber = 1, int pageSize = 10)
            => await _blockedAttemptRepository.GetBlockedAttemptsAsync(pageNumber, pageSize);
    }
}
