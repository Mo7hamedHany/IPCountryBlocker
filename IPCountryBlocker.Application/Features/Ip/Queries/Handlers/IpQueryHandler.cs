using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Application.Features.Ip.Queries.Models;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Domain.Models;
using IPCountryBlocker.Service.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Features.Ip.Queries.Handlers
{
    public class IpQueryHandler : ResponseHandler,
        IRequestHandler<GetIpLookupQuery, Response<IpGeolocation>>,
        IRequestHandler<CheckIfBlockedQuery, Response<string>>
    {
        private readonly IIpGeolocationService _ipGeolocationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly string _defaultIpAddress;

        public IpQueryHandler(IIpGeolocationService ipGeolocationService, IStringLocalizer<SharedResources> localizer, IConfiguration configuration)
            : base(localizer)
        {
            _ipGeolocationService = ipGeolocationService;
            _localizer = localizer;
            var geoConfig = configuration.GetSection("GeoLocation");
            _defaultIpAddress = geoConfig["DefaultIpAddress"]!;
        }

        public async Task<Response<IpGeolocation>> Handle(GetIpLookupQuery request, CancellationToken cancellationToken)
        {
            var ipAddress = _defaultIpAddress;
            if (request.IpAddress is not null)
                ipAddress = request.IpAddress;


            #region Deprecated Functionality

            /*This part is commented as Geolocation service reject loopback IP requests 
              And Instead I used my real local IP as default for the Caller IP address retrieval logic
            */


            //if (string.IsNullOrWhiteSpace(ipAddress))
            //{
            //    if (request.HttpContext == null)
            //        return BadRequest<IpGeolocation>(_localizer[SharedResourcesKeys.BadRequest]);

            //    ipAddress = _ipGeolocationService.GetCallerIpAddress(request.HttpContext);
            //} 
            #endregion

            if (!_ipGeolocationService.IsValidIpAddress(ipAddress))
            {
                return BadRequest<IpGeolocation>(_localizer[SharedResourcesKeys.BadRequest]);
            }

            var geolocationData = await _ipGeolocationService.GetIpGeolocationAsync(ipAddress);

            if (geolocationData == null)
                return NotFound<IpGeolocation>(_localizer[SharedResourcesKeys.NotFound]);


            return Success(geolocationData);
        }

        public async Task<Response<string>> Handle(CheckIfBlockedQuery request, CancellationToken cancellationToken)
        {
            var geolocationData = await _ipGeolocationService.GetIpGeolocationAsync(_defaultIpAddress);

            if (geolocationData == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            var isBlocked = await _ipGeolocationService.CheckIfBlockedCountry(geolocationData.CountryCode, request.UserAgent);

            if (isBlocked)
                return Success<string>(_localizer[SharedResourcesKeys.IpBlocked]);

            return Success<string>(_localizer[SharedResourcesKeys.IpAllowed]);

        }
    }
}
