using Moq;
using Xunit;
using CarStockManagementAPI.Services;
using CarStockManagementAPI.Repositories;
using CarStockManagementAPI.Models;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;

public class CarServiceTests
{
    private readonly Mock<ICarRepo> _carRepoMock;
    private readonly CarService _carService;

    public CarServiceTests()
    {
        _carRepoMock = new Mock<ICarRepo>();
        _carService = new CarService(_carRepoMock.Object);
    }
    // AddCarAsync
    [Fact]
    public async Task AddCarAsync_ShouldReturnSuccess_WhenCarDoesNotExist()
    {
        var request = new AddCarRequest
        {
            Make = "Audi",
            Model = "A4",
            Year = 2020,
            Color = "Black",
            Stock = 10
        };
        int dealerId = 1;

        _carRepoMock.Setup(r => r.GetCarByDetailsAsync(request.Make, request.Model, request.Year, request.Color, dealerId))
                    .ReturnsAsync((Car)null);

        var result = await _carService.AddCarAsync(request, dealerId);


        Assert.True(result.IsSuccess);
        Assert.Equal("Car added successfully", result.Message);
        _carRepoMock.Verify(r => r.AddCarAsync(It.IsAny<Car>()), Times.Once);
    }

    [Fact]
    public async Task AddCarAsync_ShouldReturnFailure_WhenCarAlreadyExists()
    {
        var request = new AddCarRequest
        {
            Make = "Audi",
            Model = "A4",
            Year = 2020,
            Color = "Black",
            Stock = 10
        };
        int dealerId = 1;

        var existingCar = new Car { Id = 1, Make = "Audi", Model = "A4", Year = 2020, Color = "Black", DealerId = dealerId };

        _carRepoMock.Setup(r => r.GetCarByDetailsAsync(request.Make, request.Model, request.Year, request.Color, dealerId))
                    .ReturnsAsync(existingCar);

        var result = await _carService.AddCarAsync(request, dealerId);

        Assert.False(result.IsSuccess);
        Assert.Equal("Car already exists for this dealer", result.Message);
        _carRepoMock.Verify(r => r.AddCarAsync(It.IsAny<Car>()), Times.Never);
    }

    // RemoveCarAsync
    [Fact]
    public async Task RemoveCarAsync_ShouldReturnSuccess_WhenCarExistsAndBelongsToDealer()
    {
        int carId = 1;
        int dealerId = 1;

        var car = new Car { Id = carId, DealerId = dealerId };

        _carRepoMock.Setup(r => r.GetCarByIdAsync(carId))
                    .ReturnsAsync(car);


        var result = await _carService.RemoveCarAsync(carId, dealerId);


        Assert.True(result.IsSuccess);
        Assert.Equal("Car removed successfully", result.Message);
        _carRepoMock.Verify(r => r.RemoveCarAsync(carId), Times.Once);
    }

    [Fact]
    public async Task RemoveCarAsync_ShouldReturnFailure_WhenCarDoesNotExist()
    {
        int carId = 1;
        int dealerId = 1;

        _carRepoMock.Setup(r => r.GetCarByIdAsync(carId))
                    .ReturnsAsync((Car)null);

        var result = await _carService.RemoveCarAsync(carId, dealerId);

        Assert.False(result.IsSuccess);
        Assert.Equal("Car not found or you do not have permission to delete this car", result.Message);
        _carRepoMock.Verify(r => r.RemoveCarAsync(It.IsAny<int>()), Times.Never);
    }

    // ListCarsAsync
    [Fact]
    public async Task ListCarsAsync_ShouldReturnCars_WhenDealerHasCars()
    {
        int dealerId = 1;
        var cars = new List<Car>
        {
            new Car { Make = "Audi", Model = "A4", Year = 2020, Color = "Black", Stock = 5 },
            new Car { Make = "BMW", Model = "X5", Year = 2021, Color = "White", Stock = 3 }
        };

        _carRepoMock.Setup(r => r.GetCarsByDealerIdAsync(dealerId))
                    .ReturnsAsync(cars);

        var result = await _carService.ListCarsAsync(dealerId);

        Assert.Equal(2, result.Count());
        Assert.Contains(result, car => car.Make == "Audi");
        Assert.Contains(result, car => car.Make == "BMW");
    }

    [Fact]
    public async Task ListCarsAsync_ShouldReturnEmpty_WhenDealerHasNoCars()
    {
        int dealerId = 1;
        _carRepoMock.Setup(r => r.GetCarsByDealerIdAsync(dealerId))
                    .ReturnsAsync(new List<Car>());

        var result = await _carService.ListCarsAsync(dealerId);
        Assert.Empty(result);
    }

    // UpdateCarStockAsync
    [Fact]
    public async Task UpdateCarStockAsync_ShouldUpdateStock_WhenCarBelongsToDealer()
    {
        int dealerId = 1;
        int carId = 1;
        var car = new Car { Id = carId, DealerId = dealerId, Stock = 5 };

        _carRepoMock.Setup(r => r.GetCarByIdAsync(carId)).ReturnsAsync(car);
        _carRepoMock.Setup(r => r.UpdateCarStockAsync(It.IsAny<Car>())).Returns(Task.CompletedTask);

        var result = await _carService.UpdateCarStockAsync(dealerId, carId, 10);

        Assert.True(result.IsSuccess);
        Assert.Equal("Stock updated successfully", result.Message);
    }

    [Fact]
    public async Task UpdateCarStockAsync_ShouldFail_WhenCarDoesNotBelongToDealer()
    {
        int dealerId = 1;
        int carId = 1;
        var car = new Car { Id = carId, DealerId = 2, Stock = 5 };

        _carRepoMock.Setup(r => r.GetCarByIdAsync(carId)).ReturnsAsync(car);

        var result = await _carService.UpdateCarStockAsync(dealerId, carId, 10);

        Assert.False(result.IsSuccess);
        Assert.Equal("Car not found or you do not have permission to update this car", result.Message);
    }

}