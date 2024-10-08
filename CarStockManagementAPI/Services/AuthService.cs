using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarStockManagementAPI.Repositories;
using CarStockManagementAPI.Utils;
using CarStockManagementAPI.Models;
using BCrypt.Net;

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
        public AuthService(IDealerRepo dealerRepo, IJwtTokenGenerator jwtTokenGenerator)
        {
            _dealerRepo = dealerRepo;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<(bool IsSuccess, string Message, string Token)> AuthenticateAsync(string email, string password)
        {
            var dealer = await _dealerRepo.GetDealerByEmailAsync(email);
            if (dealer == null || !BCrypt.Net.BCrypt.Verify(password, dealer.PasswordHash))
            {
                return (false, "Invalid email or password", null);
            }
            return (true, "Login Success", _jwtTokenGenerator.GenerateToken(dealer.Id.ToString()));
        }

        public async Task<(bool IsSuccess, string Message)> RegisterAsync(string name, string email, string password)
        {

            var existingDealer = await _dealerRepo.GetDealerByEmailAsync(email);
            if (existingDealer != null)
            {
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
    }
}