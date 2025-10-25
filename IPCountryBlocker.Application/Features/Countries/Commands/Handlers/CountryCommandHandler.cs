using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Application.Features.Countries.Commands.Models;
using IPCountryBlocker.Application.Resources;
using IPCountryBlocker.Service.Abstractions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace IPCountryBlocker.Application.Features.Countries.Commands.Handlers
{
    public class CountryCommandHandler : ResponseHandler,
        IRequestHandler<BlockCountryCommand, Response<string>>,
        IRequestHandler<RemoveCountryFromBlockedListCommand, Response<string>>,
        IRequestHandler<TemporalCountryBlockCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICountryService _countryService;

        public CountryCommandHandler(IStringLocalizer<SharedResources> localizer, ICountryService countryService)
            : base(localizer)
        {
            _localizer = localizer;
            _countryService = countryService;
        }

        public async Task<Response<string>> Handle(BlockCountryCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);

            var result = await _countryService.AddBlockedCountryAsync(request.Code, request.Name, null);

            if (result == "Success")
                return Success<string>(_localizer[SharedResourcesKeys.Blocked]);
            else if (result == "Exists")
                return BadRequest<string>(_localizer[SharedResourcesKeys.IsExist]);
            else
                return BadRequest<string>(_localizer[SharedResourcesKeys.AddFailed]);
        }

        public async Task<Response<string>> Handle(RemoveCountryFromBlockedListCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);

            var result = await _countryService.RemoveBlockedCountryAsync(request.Code);

            if (result == "Success")
                return Success<string>(_localizer[SharedResourcesKeys.Unblocked]);
            else if (result == "NotFound")
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            else
                return BadRequest<string>(_localizer[SharedResourcesKeys.DeletedFailed]);
        }

        public async Task<Response<string>> Handle(TemporalCountryBlockCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);

            var result = await _countryService.AddBlockedCountryAsync(request.Code, request.Name, request.Duration);

            if (result == "Success")
                return Success<string>(_localizer[SharedResourcesKeys.Blocked]);
            else if (result == "Exists")
                return BadRequest<string>(_localizer[SharedResourcesKeys.IsExist]);
            else
                return BadRequest<string>(_localizer[SharedResourcesKeys.AddFailed]);
        }
    }
}
