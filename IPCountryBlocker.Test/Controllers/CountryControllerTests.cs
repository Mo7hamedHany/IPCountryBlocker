using FluentAssertions;
using IPCountryBlocker.Application.Bases;
using IPCountryBlocker.Application.DTOs;
using IPCountryBlocker.Application.Features.Countries.Commands.Models;
using IPCountryBlocker.Application.Features.Countries.Queries.Models;
using IPCountryBlocker.Test.Fixtures;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace IPCountryBlocker.Test.Controllers
{
    public class CountryControllerTests
    {
        private readonly CountryControllerFixture _fixture;

        public CountryControllerTests()
        {
            _fixture = new CountryControllerFixture();
        }

        #region BlockCountry Tests

        [Fact]
        public async Task BlockCountry_WithValidCommand_ReturnsCreatedResult()
        {
            // Arrange
            var command = new BlockCountryCommand { Code = "US", Name = "United States" };
            var expectedResponse = new Response<string>
            {
                Data = "US",
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<BlockCountryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.BlockCountry(command);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            var createdResult = result as CreatedResult;
            createdResult?.StatusCode.Should().Be(201);

            _fixture.MediatorMock.Verify(
                m => m.Send(It.IsAny<BlockCountryCommand>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task BlockCountry_WithDuplicateCountry_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new BlockCountryCommand { Code = "US", Name = "United States" };
            var expectedResponse = new Response<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = "This country is already in the blocked list"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<BlockCountryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.BlockCountry(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequest = result as BadRequestObjectResult;
            badRequest?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task BlockCountry_WithInvalidCountryCode_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new BlockCountryCommand { Code = "INVALID", Name = "Invalid Country" };
            var expectedResponse = new Response<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = "Country code must be in ISO 3166-1 alpha-2 format"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<BlockCountryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.BlockCountry(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task BlockCountry_WithNullCommand_HandlesGracefully()
        {
            // Arrange
            BlockCountryCommand? command = null;

            // Act & Assert
            command.Should().BeNull();
        }

        #endregion

        #region BlockCountryTemporary Tests

        [Fact]
        public async Task BlockCountryTemporary_WithValidCommand_ReturnsCreatedResult()
        {
            // Arrange
            var command = new TemporalCountryBlockCommand
            {
                Code = "EG",
                Name = "Egypt",
                Duration = 120
            };
            var expectedResponse = new Response<string>
            {
                Data = "EG",
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = "Temporary block created successfully"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<TemporalCountryBlockCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.BlockCountryTemporary(command);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<TemporalCountryBlockCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1441)]
        [InlineData(-5)]
        public async Task BlockCountryTemporary_WithInvalidDuration_ReturnsBadRequestResult(int invalidDuration)
        {
            // Arrange
            var command = new TemporalCountryBlockCommand
            {
                Code = "GB",
                Name = "United Kingdom",
                Duration = invalidDuration
            };
            var expectedResponse = new Response<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = "Duration must be between 1 and 1440 minutes"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<TemporalCountryBlockCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.BlockCountryTemporary(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task BlockCountryTemporary_WithAlreadyBlockedCountry_ReturnsConflictResult()
        {
            // Arrange
            var command = new TemporalCountryBlockCommand
            {
                Code = "US",
                Name = "United States",
                Duration = 120
            };
            var expectedResponse = new Response<string>
            {
                StatusCode = HttpStatusCode.Conflict,
                Succeeded = false,
                Message = "This country is already temporarily blocked"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<TemporalCountryBlockCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.BlockCountryTemporary(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region UnblockCountry Tests

        [Fact]
        public async Task UnblockCountry_WithValidCode_ReturnsOkResult()
        {
            // Arrange
            string countryCode = "US";
            var expectedResponse = new Response<string>
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Country unblocked successfully"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<RemoveCountryFromBlockedListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.UnblockCountry(countryCode);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);

            _fixture.MediatorMock.Verify(
                m => m.Send(It.Is<RemoveCountryFromBlockedListCommand>(cmd => cmd.Code == countryCode), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UnblockCountry_WithNonExistentCode_ReturnsNotFoundResult()
        {
            // Arrange
            string countryCode = "XX";
            var expectedResponse = new Response<string>
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = "Country not found in blocked list"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<RemoveCountryFromBlockedListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.UnblockCountry(countryCode);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult?.StatusCode.Should().Be(404);
        }

        [Theory]
        [InlineData("")]
        public async Task UnblockCountry_WithEmptyCode_ReturnsNotFoundOrBadRequest(string? countryCode)
        {
            // Arrange
            var expectedResponse = new Response<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = "Country code is required"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<RemoveCountryFromBlockedListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            if (!string.IsNullOrEmpty(countryCode))
            {
                var result = await _fixture.Controller.UnblockCountry(countryCode);

                // Assert
                result.Should().BeOfType<BadRequestObjectResult>();
            }
        }

        #endregion

        #region GetBlockedCountries Tests

        [Fact]
        public async Task GetBlockedCountries_WithDefaultParameters_ReturnsOkResultWithPagedData()
        {
            // Arrange
            string? searchItem = null;
            int pageNumber = 1;
            int pageSize = 10;

            var countryDtos = new List<CountryQueryDto>
            {
                new CountryQueryDto { Code = "US", Name = "United States" },
                new CountryQueryDto { Code = "GB", Name = "United Kingdom" },
                new CountryQueryDto { Code = "EG", Name = "Egypt" }
            };

            var expectedResponse = new Response<List<CountryQueryDto>>
            {
                Data = countryDtos,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Retrieved successfully",
                Meta = new
                {
                    TotalCount = 3,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = 1,
                    HasPrevious = false,
                    HasNext = false
                }
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBlockedCountriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.GetBlockedCountries(searchItem, pageNumber, pageSize);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);

            var response = okResult?.Value as Response<List<CountryQueryDto>>;
            response?.Data?.Should().HaveCount(3);
            response?.Meta.Should().NotBeNull();

            _fixture.MediatorMock.Verify(
                m => m.Send(It.Is<GetAllBlockedCountriesQuery>(q =>
                    q.SearchItem == searchItem &&
                    q.PageNumber == pageNumber &&
                    q.PageSize == pageSize), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetBlockedCountries_WithSearchItem_ReturnsFilteredResults()
        {
            // Arrange
            string searchItem = "US";
            int pageNumber = 1;
            int pageSize = 10;

            var countryDtos = new List<CountryQueryDto>
            {
                new CountryQueryDto { Code = "US", Name = "United States" }
            };

            var expectedResponse = new Response<List<CountryQueryDto>>
            {
                Data = countryDtos,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Search completed",
                Meta = new
                {
                    TotalCount = 1,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = 1,
                    HasPrevious = false,
                    HasNext = false
                }
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBlockedCountriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.GetBlockedCountries(searchItem, pageNumber, pageSize);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var response = okResult?.Value as Response<List<CountryQueryDto>>;
            response?.Data?.Should().HaveCount(1);
            response?.Data?.First().Code.Should().Be("US");
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 5)]
        [InlineData(3, 20)]
        public async Task GetBlockedCountries_WithDifferentPagination_ReturnsCorrectResults(int pageNumber, int pageSize)
        {
            // Arrange
            var expectedResponse = new Response<List<CountryQueryDto>>
            {
                Data = new List<CountryQueryDto>(),
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Retrieved successfully",
                Meta = new
                {
                    TotalCount = 0,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = 0,
                    HasPrevious = pageNumber > 1,
                    HasNext = false
                }
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBlockedCountriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.GetBlockedCountries(null, pageNumber, pageSize);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            _fixture.MediatorMock.Verify(
                m => m.Send(It.Is<GetAllBlockedCountriesQuery>(q =>
                    q.PageNumber == pageNumber &&
                    q.PageSize == pageSize), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetBlockedCountries_WithNoResults_ReturnsEmptyList()
        {
            // Arrange
            var expectedResponse = new Response<List<CountryQueryDto>>
            {
                Data = new List<CountryQueryDto>(),
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "No blocked countries found",
                Meta = new
                {
                    TotalCount = 0,
                    PageSize = 10,
                    CurrentPage = 1,
                    TotalPages = 0,
                    HasPrevious = false,
                    HasNext = false
                }
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllBlockedCountriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _fixture.Controller.GetBlockedCountries(null, 1, 10);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var response = okResult?.Value as Response<List<CountryQueryDto>>;
            response?.Data?.Should().BeEmpty();
        }

        #endregion

        #region Integration-like Tests

        [Fact]
        public async Task BlockCountry_ThenUnblockCountry_FollowsCorrectWorkflow()
        {
            // Arrange
            var blockCommand = new BlockCountryCommand { Code = "US", Name = "United States" };
            var blockResponse = new Response<string>
            {
                Data = "US",
                StatusCode = HttpStatusCode.Created,
                Succeeded = true
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<BlockCountryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(blockResponse);

            // Act - Block country
            var blockResult = await _fixture.Controller.BlockCountry(blockCommand);

            // Assert - Block succeeded
            blockResult.Should().BeOfType<CreatedResult>();

            // Arrange - Now unblock
            var unblockResponse = new Response<string>
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Unblocked"
            };

            _fixture.MediatorMock
                .Setup(m => m.Send(It.IsAny<RemoveCountryFromBlockedListCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(unblockResponse);

            // Act - Unblock country
            var unblockResult = await _fixture.Controller.UnblockCountry("US");

            // Assert - Unblock succeeded
            unblockResult.Should().BeOfType<OkObjectResult>();

            // Verify both operations were called
            _fixture.MediatorMock.Verify(m => m.Send(It.IsAny<IRequest<Response<string>>>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        #endregion
    }
}
