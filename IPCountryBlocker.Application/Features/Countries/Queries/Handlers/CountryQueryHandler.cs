using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Application.DTOs;
using IPCountryBlocker.Application.Features.Countries.Queries.Models;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Service.Abstractions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Features.Countries.Queries.Handlers
{
    public class CountryQueryHandler : ResponseHandler,
        IRequestHandler<GetAllBlockedCountriesQuery, Response<List<CountryQueryDto>>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICountryService _countryService;

        public CountryQueryHandler(IStringLocalizer<SharedResources> localizer, ICountryService countryService)
            : base(localizer)
        {
            _localizer = localizer;
            _countryService = countryService;
        }

        public async Task<Response<List<CountryQueryDto>>> Handle(GetAllBlockedCountriesQuery request, CancellationToken cancellationToken)
        {
            var pagedResult = await _countryService.GetBlockedCountriesAsync(request.SearchItem, request.PageNumber, request.PageSize);

            if (pagedResult.Items == null || !pagedResult.Items.Any())
                return NotFound<List<CountryQueryDto>>(_localizer[SharedResourcesKeys.NotFound]);

            var countryDtos = pagedResult.Items.Select(c => new CountryQueryDto
            {
                Code = c.Code,
                Name = c.Name,
                IsTemporary = c.IsTemporary,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
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
