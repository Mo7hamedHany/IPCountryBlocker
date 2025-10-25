using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IPCountryBlocker.Application.Features.Ip.Queries.Models
{
    public class GetIpLookupQuery : IRequest<Response<IpGeolocation>>
    {
        public GetIpLookupQuery(string? ipAddress, HttpContext? httpContext)
        {
            IpAddress = ipAddress;
            HttpContext = httpContext;
        }

        public string? IpAddress { get; set; }
        public HttpContext? HttpContext { get; set; }
    }
}
