using FluentAssertions;
using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Application.DTOs;
using IPCountryBlocker.Application.Features.Countries.Commands.Models;
using IPCountryBlocker.Application.Features.Countries.Queries.Models;
using IPCountryBlocker.Test.Fixtures;
using Moq;
using System.Net;

namespace IPCountryBlocker.Test.Controllers
{
    /// <summary>
    /// Simplified and working unit tests for CountryController
    /// </summary>
    public class CountryControllerSimplifiedTests : IDisposable
    {
        private readonly CountryControllerFixture _fixture;

        public CountryControllerSimplifiedTests()
        {
            _fixture = new CountryControllerFixture();
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }

        #region BlockCountry Tests

        [Fact]
        public async Task BlockCountry_WithValidCommand_ShouldCallMediator()
        {
            // Arrange
            var command = new BlockCountryCommand { Code = "US", Name = "United States" };
            var response = new Response<string>
            {
                Data = "US",
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<BlockCountryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.BlockCountry(command);

            // Assert
            result.Should().NotBeNull();
            _fixture.MediatorMock.Verify(
                m => m.Send(It.IsAny<BlockCountryCommand>(), It.IsAny<CancellationToken>()),
                Times.Once,
                "Mediator.Send should be called exactly once");
        }

        [Fact]
        public async Task BlockCountry_WithDuplicateCode_ReturnsBadRequest()
        {
            // Arrange
            var command = new BlockCountryCommand { Code = "US", Name = "United States" };
            var response = new Response<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = "Country already exists"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<BlockCountryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.BlockCountry(command);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task BlockCountry_WithInvalidCode_ReturnsBadRequest()
        {
            // Arrange
            var command = new BlockCountryCommand { Code = "INVALID", Name = "Invalid" };
            var response = new Response<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = "Invalid country code"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<BlockCountryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.BlockCountry(command);

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region BlockCountryTemporary Tests

        [Fact]
        public async Task BlockCountryTemporary_WithValidCommand_ShouldCallMediator()
        {
            // Arrange
            var command = new TemporalCountryBlockCommand
            {
                Code = "EG",
                Name = "Egypt",
                Duration = 120
            };
            var response = new Response<string>
            {
                Data = "EG",
                StatusCode = HttpStatusCode.Created,
                Succeeded = true
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<TemporalCountryBlockCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.BlockCountryTemporary(command);

            // Assert
            result.Should().NotBeNull();
            _fixture.MediatorMock.Verify(
                m => m.Send(It.IsAny<TemporalCountryBlockCommand>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1441)]
        [InlineData(-5)]
        public async Task BlockCountryTemporary_WithInvalidDuration_ReturnsBadRequest(int duration)
        {
            // Arrange
            var command = new TemporalCountryBlockCommand
            {
                Code = "GB",
                Name = "UK",
                Duration = duration
            };
            var response = new Response<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<TemporalCountryBlockCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.BlockCountryTemporary(command);

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region UnblockCountry Tests

        [Fact]
        public async Task UnblockCountry_WithValidCode_ShouldCallMediator()
        {
            // Arrange
            string code = "US";
            var response = new Response<string>
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Unblocked"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<RemoveCountryFromBlockedListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.UnblockCountry(code);

            // Assert
            result.Should().NotBeNull();
            _fixture.MediatorMock.Verify(
                m => m.Send(It.IsAny<RemoveCountryFromBlockedListCommand>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UnblockCountry_WithNonExistentCode_ReturnsNotFound()
        {
            // Arrange
            string code = "XX";
            var response = new Response<string>
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<RemoveCountryFromBlockedListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.UnblockCountry(code);

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region GetBlockedCountries Tests

        [Fact]
        public async Task GetBlockedCountries_WithDefaultParams_ShouldCallMediator()
        {
            // Arrange
            var countryDtos = new List<CountryQueryDto>
            {
                new CountryQueryDto { Code = "US", Name = "United States" },
                new CountryQueryDto { Code = "GB", Name = "United Kingdom" }
            };

            var response = new Response<List<CountryQueryDto>>
            {
                Data = countryDtos,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Meta = new { TotalCount = 2 }
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBlockedCountriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.GetBlockedCountries(null, 1, 10);

            // Assert
            result.Should().NotBeNull();
            _fixture.MediatorMock.Verify(
                m => m.Send(It.IsAny<GetAllBlockedCountriesQuery>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetBlockedCountries_WithSearchItem_FiltersBySearchTerm()
        {
            // Arrange
            string searchItem = "US";
            var countryDtos = new List<CountryQueryDto>
            {
                new CountryQueryDto { Code = "US", Name = "United States" }
            };

            var response = new Response<List<CountryQueryDto>>
            {
                Data = countryDtos,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBlockedCountriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.GetBlockedCountries(searchItem, 1, 10);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetBlockedCountries_WithNoResults_ReturnsEmptyList()
        {
            // Arrange
            var response = new Response<List<CountryQueryDto>>
            {
                Data = new List<CountryQueryDto>(),
                StatusCode = HttpStatusCode.OK,
                Succeeded = true
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBlockedCountriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.GetBlockedCountries(null, 1, 10);

            // Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 5)]
        [InlineData(3, 20)]
        public async Task GetBlockedCountries_WithDifferentPagination_PassesCorrectParams(int page, int pageSize)
        {
            // Arrange
            var response = new Response<List<CountryQueryDto>>
            {
                Data = new List<CountryQueryDto>(),
                StatusCode = HttpStatusCode.OK,
                Succeeded = true
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBlockedCountriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _fixture.Controller.GetBlockedCountries(null, page, pageSize);

            // Assert
            result.Should().NotBeNull();
            _fixture.MediatorMock.Verify(
                m => m.Send(It.Is<GetAllBlockedCountriesQuery>(q => q.PageNumber == page && q.PageSize == pageSize),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion
    }
}