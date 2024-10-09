#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Models;
using CarStockManagementAPI.Repositories;
using Microsoft.Extensions.Logging;

namespace CarStockManagementAPI.Services
{
    public interface ICarService
    {
        Task<(bool IsSuccess, string Message)> AddCarAsync(AddCarRequest repCar, int dealerId);
        Task<(bool IsSuccess, string Message)> RemoveCarAsync(int carId, int dealerId);
        Task<IEnumerable<Car>> ListCarsAsync(int dealerId);
        Task<(bool IsSuccess, string Message)> UpdateCarStockAsync(int dealerId, int carId, int newStock);
        Task<IEnumerable<CarResponse>> SearchCarsAsync(int dealerId, string make, string? model);
    }

    public class CarService : ICarService
    {
        private readonly ICarRepo _carRepo;
        private readonly ILogger<CarService> _logger;

        public CarService(ICarRepo carRepo, ILogger<CarService> logger)
        {
            _carRepo = carRepo;
            _logger = logger;
        }

        public async Task<(bool IsSuccess, string Message)> AddCarAsync(AddCarRequest repCar, int dealerId)
        {
            try
            {
                var existingCar = await _carRepo.GetCarByDetailsAsync(repCar.Make, repCar.Model, repCar.Year, repCar.Color, dealerId);
                if (existingCar != null)
                {
                    _logger.LogWarning("Attempt to add a duplicate car for DealerId: {DealerId}, Make: {Make}, Model: {Model}", dealerId, repCar.Make, repCar.Model);
                    return (false, "Car already exists for this dealer");
                }

                var car = new Car
                {
                    Make = repCar.Make,
                    Model = repCar.Model,
                    Year = repCar.Year,
                    Color = repCar.Color,
                    Stock = repCar.Stock,
                    DealerId = dealerId
                };

                await _carRepo.AddCarAsync(car);
                return (true, "Car added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a car for DealerId: {DealerId}, Make: {Make}, Model: {Model}", dealerId, repCar.Make, repCar.Model);
                return (false, "An error occurred while adding the car");
            }
        }

        public async Task<IEnumerable<Car>> ListCarsAsync(int dealerId)
        {
            try
            {
                return await _carRepo.GetCarsByDealerIdAsync(dealerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while listing cars for DealerId: {DealerId}", dealerId);
                return Enumerable.Empty<Car>();
            }
        }

        public async Task<(bool IsSuccess, string Message)> RemoveCarAsync(int carId, int dealerId)
        {
            try
            {
                var car = await _carRepo.GetCarByIdAsync(carId);
                if (car == null || car.DealerId != dealerId)
                {
                    _logger.LogWarning("Attempt to remove a car that does not belong to DealerId: {DealerId}", dealerId);
                    return (false, "Car not found or you do not have permission to delete this car");
                }
                await _carRepo.RemoveCarAsync(carId);
                return (true, "Car removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing car with CarId: {CarId}, DealerId: {DealerId}", carId, dealerId);
                return (false, "An error occurred while removing the car");
            }
        }

        public async Task<IEnumerable<CarResponse>> SearchCarsAsync(int dealerId, string make, string? model)
        {
            try
            {
                var cars = await _carRepo.SearchCarsAsync(dealerId, make, model);
                return cars.Select(car => new CarResponse
                {
                    CarId = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    Color = car.Color,
                    Stock = car.Stock
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching cars for DealerId: {DealerId}, Make: {Make}, Model: {Model}", dealerId, make, model);
                return Enumerable.Empty<CarResponse>();
            }
        }

        public async Task<(bool IsSuccess, string Message)> UpdateCarStockAsync(int dealerId, int carId, int newStock)
        {
            try
            {
                var car = await _carRepo.GetCarByIdAsync(carId);
                if (car == null || car.DealerId != dealerId)
                {
                    _logger.LogWarning("Attempt to update stock for a car that does not belong to DealerId: {DealerId}", dealerId);
                    return (false, "Car not found or you do not have permission to update this car");
                }
                car.Stock = newStock;
                await _carRepo.UpdateCarStockAsync(car);
                return (true, "Stock updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating stock for CarId: {CarId}, DealerId: {DealerId}", carId, dealerId);
                return (false, "An error occurred while updating the car stock");
            }
        }
    }
}