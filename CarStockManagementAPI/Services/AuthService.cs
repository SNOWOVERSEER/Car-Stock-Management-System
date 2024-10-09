using System;
using System.Threading.Tasks;
using CarStockManagementAPI.Repositories;
using CarStockManagementAPI.Utils;
using CarStockManagementAPI.Models;
using Microsoft.Extensions.Logging;

namespace CarStockManagementAPI.Services
{
    public interface IAuthService
    {
        Task<(bool IsSuccess, string Message, string Token)> AuthenticateAsync(string email, string password);
        Task<(bool IsSuccess, string Message)> RegisterAsync(string name, string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IDealerRepo _dealerRepo;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IDealerRepo dealerRepo, IJwtTokenGenerator jwtTokenGenerator, ILogger<AuthService> logger)
        {
            _dealerRepo = dealerRepo;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, string Message, string Token)> AuthenticateAsync(string email, string password)
        {
            try
            {
                var dealer = await _dealerRepo.GetDealerByEmailAsync(email);
                if (dealer == null || !BCrypt.Net.BCrypt.Verify(password, dealer.PasswordHash))
                {
                    _logger.LogWarning("Authentication failed for Email: {Email} - Invalid email or password", email);
                    return (false, "Invalid email or password", null);
                }

                var token = _jwtTokenGenerator.GenerateToken(dealer.Id.ToString());
                return (true, "Login Success", token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating Email: {Email}", email);
                return (false, "An error occurred while authenticating", null);
            }
        }

        public async Task<(bool IsSuccess, string Message)> RegisterAsync(string name, string email, string password)
        {
            try
            {
                var existingDealer = await _dealerRepo.GetDealerByEmailAsync(email);
                if (existingDealer != null)
                {
                    _logger.LogWarning("Registration failed for Email: {Email} - Email already exists", email);
                    return (false, "Email already exists");
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                var dealer = new Dealer
                {
                    Name = name,
                    Email = email,
                    PasswordHash = passwordHash
                };

                await _dealerRepo.AddDealerAsync(dealer);
                return (true, "Registration Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering Email: {Email}", email);
                return (false, "An error occurred while registering");
            }
        }
    }
}