#nullable enable
using System;
using System.Data;
using System.Threading.Tasks;
using CarStockManagementAPI.Models;
using Dapper;
using Microsoft.Extensions.Logging;

namespace CarStockManagementAPI.Repositories
{
    public interface IDealerRepo
    {
        Task AddDealerAsync(Dealer dealer);
        Task<Dealer?> GetDealerByEmailAsync(string email);
    }

    public class DealerRepo : IDealerRepo
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<DealerRepo> _logger;

        public DealerRepo(IDbConnection connection, ILogger<DealerRepo> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task AddDealerAsync(Dealer dealer)
        {
            try
            {
                var query = @"INSERT INTO Dealer (Name, Email, PasswordHash) VALUES (@Name, @Email, @PasswordHash)";
                await _connection.ExecuteAsync(query, new { dealer.Name, dealer.Email, dealer.PasswordHash });
                _logger.LogInformation("Dealer added successfully: {Name}, Email: {Email}", dealer.Name, dealer.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding dealer: {Name}, Email: {Email}", dealer.Name, dealer.Email);
                throw new Exception("Error occurred while adding dealer to the database.", ex);
            }
        }

        public async Task<Dealer?> GetDealerByEmailAsync(string email)
        {
            try
            {
                var query = @"SELECT * FROM Dealer WHERE Email = @Email";
                var dealer = await _connection.QueryFirstOrDefaultAsync<Dealer>(query, new { Email = email });

                if (dealer != null)
                {
                    _logger.LogInformation("Dealer found: {Email}", email);
                }

                return dealer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving dealer by email: {Email}", email);
                throw new Exception($"Error occurred while retrieving dealer by email {email}.", ex);
            }
        }
    }
}