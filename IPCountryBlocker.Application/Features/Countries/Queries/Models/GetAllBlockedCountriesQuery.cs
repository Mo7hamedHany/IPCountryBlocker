using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Application.DTOs;
using MediatR;

namespace IPCountryBlocker.Application.Features.Countries.Queries.Models
{
    public class GetAllBlockedCountriesQuery : IRequest<Response<List<CountryQueryDto>>>
    {
        public GetAllBlockedCountriesQuery(string? searchItem, int pageNumber, int pageSize)
        {
            SearchItem = searchItem;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public string? SearchItem { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
