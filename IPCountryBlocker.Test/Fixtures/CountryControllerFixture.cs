using IPCountryBlocker.API.Controllers;
using IPCountryBlocker.Test.Helpers;
using MediatR;
using Moq;

namespace IPCountryBlocker.Test.Fixtures
{
    public class CountryControllerFixture : IDisposable
    {
        public Mock<IMediator> MediatorMock { get; }
        public CountryController Controller { get; }

        public CountryControllerFixture()
        {
            MediatorMock = new Mock<IMediator>();
            Controller = new CountryController();

            // Inject the mock mediator
            MockSetupHelper.InjectMediator(Controller, MediatorMock);
        }

        public void Reset()
        {
            MediatorMock.Reset();
        }

        public void Dispose()
        {
            // Mock objects don't need explicit disposal in most cases
            // but we can clear the setup if needed
            MediatorMock?.Reset();
            GC.SuppressFinalize(this);
        }
    }
}
