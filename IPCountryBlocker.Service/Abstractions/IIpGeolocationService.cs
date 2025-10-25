using IPCountryBlocker.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace IPCountryBlocker.Service.Abstractions
{
    public interface IIpGeolocationService
    {
        Task<IpGeolocation?> GetIpGeolocationAsync(string ipAddress);
        bool IsValidIpAddress(string ipAddress);
        string GetCallerIpAddress(HttpContext httpContext);
        Task<bool> CheckIfBlockedCountry(string countryCode, string? userAgent);
    }
}
