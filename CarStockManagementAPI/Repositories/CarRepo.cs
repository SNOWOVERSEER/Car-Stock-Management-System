using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Models;
using Dapper;

namespace CarStockManagementAPI.Repositories
{
    public interface ICarRepo
    {
        Task AddCarAsync(Car car);
        Task<Car> GetCarByIdAsync(int id);
        Task RemoveCarAsync(int id);
        Task<Car> GetCarByDetailsAsync(string make, string model, int year, string color, int dealerId);
    }
    public class CarRepo : ICarRepo
    {
        private readonly IDbConnection _connection;
        public CarRepo(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task AddCarAsync(Car car)
        {
            var query = @"INSERT INTO Car (Make, Model, Year, Color, Stock, DealerId) VALUES (@Make, @Model, @Year, @Color, @Stock, @DealerId)";
            await _connection.ExecuteAsync(query, new { car.Make, car.Model, car.Year, car.Color, car.Stock, car.DealerId });
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            var query = @"SELECT * FROM Car WHERE Id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<Car>(query, new { Id = id });
        }

        public async Task<Car> GetCarByDetailsAsync(string make, string model, int year, string color, int dealerId)
        {
            var query = @"SELECT * FROM Car WHERE Make = @Make AND Model = @Model AND Year = @Year AND Color = @Color AND DealerId = @DealerId";
            return await _connection.QuerySingleOrDefaultAsync<Car>(query, new { Make = make, Model = model, Year = year, Color = color, DealerId = dealerId });
        }

        public async Task RemoveCarAsync(int id)
        {
            var query = @"DELETE FROM Car WHERE Id = @Id";
            await _connection.ExecuteAsync(query, new { Id = id });
        }
    }
}