#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CarStockManagementAPI.Models;
using Dapper;
using Microsoft.Extensions.Logging;

namespace CarStockManagementAPI.Repositories
{
    public interface ICarRepo
    {
        Task AddCarAsync(Car car);
        Task<Car?> GetCarByIdAsync(int id);
        Task RemoveCarAsync(int id);
        Task<Car?> GetCarByDetailsAsync(string make, string model, int year, string color, int dealerId);
        Task<IEnumerable<Car>> GetCarsByDealerIdAsync(int dealerId);
        Task UpdateCarStockAsync(Car car);
        Task<IEnumerable<Car>> SearchCarsAsync(int dealerId, string make, string? model);
    }

    public class CarRepo : ICarRepo
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<CarRepo> _logger;

        public CarRepo(IDbConnection connection, ILogger<CarRepo> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task AddCarAsync(Car car)
        {
            try
            {
                var query = @"INSERT INTO Car (Make, Model, Year, Color, Stock, DealerId) VALUES (@Make, @Model, @Year, @Color, @Stock, @DealerId)";
                await _connection.ExecuteAsync(query, new { car.Make, car.Model, car.Year, car.Color, car.Stock, car.DealerId });
                _logger.LogInformation("Car added successfully: {Make} {Model}, DealerId: {DealerId}", car.Make, car.Model, car.DealerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a car to the database.");
                throw new Exception("Error occurred while adding a car to the database.", ex);
            }
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            try
            {
                var query = @"SELECT * FROM Car WHERE Id = @Id";
                var car = await _connection.QuerySingleOrDefaultAsync<Car>(query, new { Id = id });
                _logger.LogInformation("Car retrieved by ID: {Id}", id);
                return car;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving car with ID {Id}.", id);
                throw new Exception($"Error occurred while retrieving car with ID {id}.", ex);
            }
        }

        public async Task<Car?> GetCarByDetailsAsync(string make, string model, int year, string color, int dealerId)
        {
            try
            {
                var query = @"SELECT * FROM Car WHERE Make = @Make AND Model = @Model AND Year = @Year AND Color = @Color AND DealerId = @DealerId";
                var car = await _connection.QuerySingleOrDefaultAsync<Car>(query, new { Make = make, Model = model, Year = year, Color = color, DealerId = dealerId });
                _logger.LogInformation("Car retrieved by details: {Make} {Model}, DealerId: {DealerId}", make, model, dealerId);
                return car;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving car by details.");
                throw new Exception("Error occurred while retrieving car by details.", ex);
            }
        }

        public async Task RemoveCarAsync(int id)
        {
            try
            {
                var query = @"DELETE FROM Car WHERE Id = @Id";
                await _connection.ExecuteAsync(query, new { Id = id });
                _logger.LogInformation("Car removed: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing car with ID {Id}.", id);
                throw new Exception($"Error occurred while removing car with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Car>> GetCarsByDealerIdAsync(int dealerId)
        {
            try
            {
                var query = @"SELECT * FROM Car WHERE DealerId = @DealerId";
                var cars = await _connection.QueryAsync<Car>(query, new { DealerId = dealerId });
                _logger.LogInformation("Cars retrieved for DealerId: {DealerId}", dealerId);
                return cars;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving cars for DealerId {DealerId}.", dealerId);
                throw new Exception($"Error occurred while retrieving cars for DealerId {dealerId}.", ex);
            }
        }

        public async Task UpdateCarStockAsync(Car car)
        {
            try
            {
                var query = @"UPDATE Car SET Stock = @Stock WHERE Id = @Id AND DealerId = @DealerId";
                await _connection.ExecuteAsync(query, new { car.Stock, car.Id, car.DealerId });
                _logger.LogInformation("Car stock updated: {Id}, New Stock: {Stock}", car.Id, car.Stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating car stock for ID {Id}.", car.Id);
                throw new Exception($"Error occurred while updating car stock for ID {car.Id}.", ex);
            }
        }

        public async Task<IEnumerable<Car>> SearchCarsAsync(int dealerId, string make, string? model)
        {
            try
            {
                string query;
                object parameters;

                if (string.IsNullOrEmpty(model))
                {
                    query = @"SELECT * FROM Car WHERE DealerId = @DealerId AND Make = @Make";
                    parameters = new { DealerId = dealerId, Make = make };
                }
                else
                {
                    query = @"SELECT * FROM Car WHERE DealerId = @DealerId AND Make = @Make AND Model = @Model";
                    parameters = new { DealerId = dealerId, Make = make, Model = model };
                }

                var cars = await _connection.QueryAsync<Car>(query, parameters);
                _logger.LogInformation("Cars retrieved for DealerId: {DealerId}, Make: {Make}, Model: {Model}", dealerId, make, model ?? "Any");
                return cars;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching cars for DealerId {DealerId}.", dealerId);
                throw new Exception($"Error occurred while searching cars for DealerId {dealerId}.", ex);
            }
        }
    }
}