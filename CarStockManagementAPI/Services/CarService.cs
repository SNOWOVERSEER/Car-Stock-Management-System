using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Models;
using CarStockManagementAPI.Repositories;

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
        public CarService(ICarRepo carRepo)
        {
            _carRepo = carRepo;
        }

        public async Task<(bool IsSuccess, string Message)> AddCarAsync(AddCarRequest repCar, int dealerId)
        {
            var car = new Car
            {
                Make = repCar.Make,
                Model = repCar.Model,
                Year = repCar.Year,
                Color = repCar.Color,
                Stock = repCar.Stock,
                DealerId = dealerId
            };

            var existingCar = await _carRepo.GetCarByDetailsAsync(car.Make, car.Model, car.Year, car.Color, car.DealerId);
            if (existingCar != null)
            {
                return (false, "Car already exists for this dealer");
            }

            await _carRepo.AddCarAsync(car);
            return (true, "Car added successfully");

        }

        public async Task<IEnumerable<Car>> ListCarsAsync(int dealerId)
        {
            return await _carRepo.GetCarsByDealerIdAsync(dealerId);
        }

        public async Task<(bool IsSuccess, string Message)> RemoveCarAsync(int carId, int dealerId)
        {
            var car = await _carRepo.GetCarByIdAsync(carId);
            if (car == null || car.DealerId != dealerId)
            {
                return (false, "Car not found or you do not have permission to delete this car");
            }
            await _carRepo.RemoveCarAsync(carId);
            return (true, "Car removed successfully");
        }

        public async Task<IEnumerable<CarResponse>> SearchCarsAsync(int dealerId, string make, string? model)
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

        public async Task<(bool IsSuccess, string Message)> UpdateCarStockAsync(int dealerId, int carId, int newStock)
        {
            var car = await _carRepo.GetCarByIdAsync(carId);
            if (car == null || car.DealerId != dealerId)
            {
                return (false, "Car not found or you do not have permission to update this car");
            }
            car.Stock = newStock;
            await _carRepo.UpdateCarStockAsync(car);
            return (true, "Stock updated successfully");
        }
    }
}