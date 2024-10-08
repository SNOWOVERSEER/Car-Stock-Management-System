using Xunit;
using Moq;
using CarStockManagementAPI.Services;
using CarStockManagementAPI.Repositories;
using CarStockManagementAPI.Models;
using CarStockManagementAPI.Utils;
using System.Threading;
using System.Threading.Tasks;


namespace CarStockManagementAPI.Tests.UnitTests
{
    public class AuthServiceTests
    {
        private readonly Mock<IDealerRepo> _dealerRepoMock;
        private readonly AuthService _authService;
        public AuthServiceTests()
        {
            _dealerRepoMock = new Mock<IDealerRepo>();
            var tokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            tokenGeneratorMock.Setup(t => t.GenerateToken(It.IsAny<string>())).Returns("fake-token");
            _authService = new AuthService(_dealerRepoMock.Object, tokenGeneratorMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            var existingDealer = new Dealer { Email = "existing@example.com" };
            _dealerRepoMock.Setup(repo => repo.GetDealerByEmailAsync(existingDealer.Email))
                           .ReturnsAsync(existingDealer);

            var result = await _authService.RegisterAsync("John", existingDealer.Email, "password");
            Assert.False(result.IsSuccess);
            Assert.Equal("Email already exists", result.Message);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccess_WhenDealerIsNew()
        {
            _dealerRepoMock.Setup(repo => repo.GetDealerByEmailAsync(It.IsAny<string>()))
                           .ReturnsAsync((Dealer)null);

            var result = await _authService.RegisterAsync("John", "john@example.com", "password");

            Assert.True(result.IsSuccess);
            Assert.Equal("Registration Success", result.Message);
        }


    }
}