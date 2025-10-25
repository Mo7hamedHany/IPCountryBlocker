using IPCountryBlocker.Application.Bases;
using MediatR;

namespace IPCountryBlocker.Application.Features.Countries.Commands.Models
{
    public class RemoveCountryFromBlockedListCommand : IRequest<Response<string>>
    {
        public string Code { get; set; }

        public RemoveCountryFromBlockedListCommand(string code)
        {
            Code = code;
        }
    }
}
