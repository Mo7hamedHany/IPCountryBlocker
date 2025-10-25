using IPCountryBlocker.Domain.Interfaces;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;

namespace IPCountryBlocker.Service.Implementations
{
    public class IpGeolocationService : IIpGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly string _ipAddress;
        private readonly ICountryRepository _countryRepository;
        private readonly IBlockedAttemptRepository _blockedAttemptRepository;

        public IpGeolocationService(HttpClient httpClient, IConfiguration configuration, ICountryRepository countryRepository,
            IBlockedAttemptRepository blockedAttemptRepository)
        {
            _httpClient = httpClient;
            var geoConfig = configuration.GetSection("GeoLocation");
            _baseUrl = geoConfig["BaseUrl"] ?? "https://api.ipgeolocation.io/ipgeo";
            _apiKey = geoConfig["ApiKey"] ?? string.Empty;
            _ipAddress = geoConfig["DefaultIpAddress"]!;
            _countryRepository = countryRepository;
            _blockedAttemptRepository = blockedAttemptRepository;
        }

        public async Task<bool> CheckIfBlockedCountry(string countryCode, string? userAgent)
        {
            var blockedCountry = await _countryRepository.GetBlockedCountryAsync(countryCode);

            var blockedAttempt = new BlockedAttempt
            {
                IpAddress = _ipAddress,
                CountryCode = countryCode,
                UserAgent = userAgent ?? string.Empty,
                CountryName = blockedCountry?.Name ?? string.Empty,
                Timestamp = DateTime.UtcNow,
                IsBlocked = blockedCountry != null,
                BlockedStatus = blockedCountry != null ? "Blocked" : "Allowed"
            };

            await _blockedAttemptRepository.AddBlockedAttemptAsync(blockedAttempt);

            return blockedCountry != null;
        }

        public string GetCallerIpAddress(HttpContext httpContext)
        {
            var forwarded = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
            {
                return forwarded.Split(',')[0].Trim();
            }

            var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
            {
                return realIp;
            }

            return httpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        }

        public async Task<IpGeolocation?> GetIpGeolocationAsync(string ipAddress)
        {
            try
            {
                if (!IsValidIpAddress(ipAddress))
                    return null;


                string url = BuildUrl(ipAddress);


                var response = await _httpClient.GetAsync(url);

                var content = await response.Content.ReadAsStringAsync();

                var jsonObject = JObject.Parse(content);


                var geolocation = new IpGeolocation
                {
                    Ip = jsonObject["ip"]?.Value<string>() ?? ipAddress,
                    CountryCode = jsonObject["country_code"]?.Value<string>() ?? jsonObject["country_code2"]?.Value<string>() ?? string.Empty,
                    CountryName = jsonObject["country_name"]?.Value<string>() ?? string.Empty,
                    City = jsonObject["city"]?.Value<string>() ?? string.Empty,
                    Region = jsonObject["state_prov"]?.Value<string>() ?? jsonObject["region"]?.Value<string>() ?? string.Empty,
                    Latitude = jsonObject["latitude"]?.Value<double>() ?? 0,
                    Longitude = jsonObject["longitude"]?.Value<double>() ?? 0,
                    Isp = jsonObject["isp"]?.Value<string>() ?? string.Empty,
                    Org = jsonObject["organization"]?.Value<string>() ?? jsonObject["org"]?.Value<string>() ?? string.Empty
                };

                return geolocation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool IsValidIpAddress(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;

            return IPAddress.TryParse(ipAddress, out var address) &&
                   (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ||
                    address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
        }

        private string BuildUrl(string ipAddress)
        {
            if (_baseUrl.Contains("ipgeolocation.io"))
            {
                return $"{_baseUrl}?apiKey={_apiKey}&ip={ipAddress}";
            }
            else
            {
                var url = $"{_baseUrl.TrimEnd('/')}/{ipAddress}/json/";
                if (!string.IsNullOrWhiteSpace(_apiKey))
                {
                    url += $"?key={_apiKey}";
                }
                return url;
            }
        }
    }
}
