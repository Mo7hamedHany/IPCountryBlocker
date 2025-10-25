using IPCountryBlocker.Application.Bases;
using MediatR;

namespace IPCountryBlocker.Application.Features.Ip.Queries.Models
{
    public class CheckIfBlockedQuery : IRequest<Response<string>>
    {
        public CheckIfBlockedQuery(string? userAgent)
        {
            UserAgent = userAgent;
        }

        public string? UserAgent { get; set; }
    }
}
