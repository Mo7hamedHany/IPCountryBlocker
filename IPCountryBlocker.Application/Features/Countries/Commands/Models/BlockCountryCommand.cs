using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Application.DTOs;
using MediatR;

namespace IPCountryBlocker.Application.Features.Countries.Commands.Models
{
    public class BlockCountryCommand : CountryDto, IRequest<Response<string>>
    {
    }
}
