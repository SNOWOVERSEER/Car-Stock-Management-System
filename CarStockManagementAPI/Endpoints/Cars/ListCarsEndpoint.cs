using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Services;
using FastEndpoints;

namespace CarStockManagementAPI.Endpoints.Cars
{
    public class ListCarsEndpoint : EndpointWithoutRequest
    {
        private readonly ICarService _carService;
        public ListCarsEndpoint(ICarService carService)
        {
            _carService = carService;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/api/cars/list");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var dealerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (dealerId == null)
                {
                    await SendAsync(new { Message = "Unauthorized" }, 401);
                    return;
                }

                var cars = await _carService.ListCarsAsync(int.Parse(dealerId));

                var carResponses = cars.Select(car => new CarResponse
                {
                    CarId = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    Color = car.Color,
                    Stock = car.Stock
                });

                var response = new ListCarsResponse
                {
                    Message = cars.Any() ? "Cars found" : "No cars found",
                    Cars = carResponses
                };

                await SendOkAsync(response);
            }
            catch
            {
                await SendAsync(new { Message = "An unexpected error occurred." }, 500);
            }
        }
    }
}