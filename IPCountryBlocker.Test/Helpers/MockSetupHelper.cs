using IPCountryBlocker.API.Bases;
using System.Reflection;

namespace IPCountryBlocker.Test.Helpers
{
    public static class MockSetupHelper
    {
        /// <summary>
        /// Injects the mock mediator into the controller via reflection
        /// </summary>
        public static void InjectMediator<TController>(TController controller, Moq.Mock<MediatR.IMediator> mediatorMock)
            where TController : AppControllerBase
        {
            var property = typeof(AppControllerBase).GetProperty("Mediator",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (property?.CanWrite == true)
            {
                property.SetValue(controller, mediatorMock.Object);
            }
        }
    }
}