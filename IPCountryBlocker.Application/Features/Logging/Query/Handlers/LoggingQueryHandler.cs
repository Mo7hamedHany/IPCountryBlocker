using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Application.Features.Logging.Query.Models;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Features.Logging.Query.Handlers
{
    public class LoggingQueryHandler : ResponseHandler,
        IRequestHandler<GetBlockedAttemptsQuery, Response<List<BlockedAttempt>>>
    {
        private readonly ILoggingService _loggingService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public LoggingQueryHandler(ILoggingService loggingService, IStringLocalizer<SharedResources> localizer)
            : base(localizer)
        {
            _loggingService = loggingService;
            _localizer = localizer;
        }

        public async Task<Response<List<BlockedAttempt>>> Handle(GetBlockedAttemptsQuery request, CancellationToken cancellationToken)
        {
            var pagedResult = await _loggingService.GetBlockedAttemptsAsync(request.PageNumber, request.PageSize);

            if (pagedResult.Items == null || !pagedResult.Items.Any())
                return NotFound<List<BlockedAttempt>>(_localizer[SharedResourcesKeys.NotFound]);

            var countryDtos = pagedResult.Items.Select(c => new BlockedAttempt
            {
                CountryCode = c.CountryCode,
                CountryName = c.CountryName,
                IsBlocked = c.IsBlocked,
                IpAddress = c.IpAddress,
                UserAgent = c.UserAgent,
                BlockedStatus = c.BlockedStatus,
                Timestamp = c.Timestamp,
            }).ToList();

            var response = Success(countryDtos);
            response.Meta = new
            {
                TotalCount = pagedResult.TotalCount,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                TotalPages = (int)Math.Ceiling(pagedResult.TotalCount / (double)request.PageSize),
                HasPrevious = request.PageNumber > 1,
                HasNext = request.PageNumber < (int)Math.Ceiling(pagedResult.TotalCount / (double)request.PageSize)
            };

            return response;
        }
    }
}
