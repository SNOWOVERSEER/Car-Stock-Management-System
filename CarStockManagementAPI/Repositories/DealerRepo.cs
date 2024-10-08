using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CarStockManagementAPI.Data;
using CarStockManagementAPI.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CarStockManagementAPI.Repositories
{
    public interface IDealerRepo
    {
        Task AddDealerAsync(Dealer dealer);
        Task<Dealer> GetDealerByEmailAsync(string email);
    }
    public class DealerRepo : IDealerRepo
    {
        private readonly IDbConnection _connection;

        public DealerRepo(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task AddDealerAsync(Dealer dealer)
        {
            var query = @"INSERT INTO Dealer (Name, Email, PasswordHash) VALUES (@Name, @Email, @PasswordHash)";
            await _connection.ExecuteAsync(query, new { dealer.Name, dealer.Email, dealer.PasswordHash });
        }

        public async Task<Dealer> GetDealerByEmailAsync(string email)
        {
            var query = @"SELECT * FROM Dealer WHERE Email = @Email";
            return await _connection.QueryFirstOrDefaultAsync<Dealer>(query, new { Email = email });
        }
    }

}