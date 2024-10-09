using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using CarStockManagementAPI.Models;

namespace CarStockManagementAPI.Data
{
    public class CarDealershipDbContext
    {
        private readonly string _connectionString;
        public CarDealershipDbContext(string connectionString)
        {
            _connectionString = connectionString;
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var createDealerTable = @"CREATE TABLE IF NOT EXISTS Dealer (
            Id INTEGER PRIMARY KEY,
            Name TEXT NOT NULL,
            Email TEXT NOT NULL,
            PasswordHash TEXT NOT NULL
        )";

            connection.Execute(createDealerTable);

            var createCarTable = @"CREATE TABLE IF NOT EXISTS Car (
            Id INTEGER PRIMARY KEY,
            Make TEXT NOT NULL,
            Model TEXT NOT NULL,
            Year INTEGER NOT NULL,
            Color TEXT NOT NULL,
            Stock INTEGER NOT NULL,
            DealerId INTEGER NOT NULL,
            FOREIGN KEY (DealerId) REFERENCES Dealer(Id)
        )";

            connection.Execute(createCarTable);

        }

    }
}