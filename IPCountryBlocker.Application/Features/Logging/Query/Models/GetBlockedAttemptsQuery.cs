using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Domain.Models;
using MediatR;

namespace IPCountryBlocker.Application.Features.Logging.Query.Models
{
    public class GetBlockedAttemptsQuery : IRequest<Response<List<BlockedAttempt>>>
    {
        public GetBlockedAttemptsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
