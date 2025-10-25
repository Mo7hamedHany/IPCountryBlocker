using IPCountryBlocker.Application.Bases;
using MediatR;

namespace IPCountryBlocker.Application.Features.Countries.Commands.Models
{
    public class TemporalCountryBlockCommand : IRequest<Response<string>>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
    }
}
