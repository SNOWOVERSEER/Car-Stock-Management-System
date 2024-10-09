using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Services;
using FastEndpoints;

namespace CarStockManagementAPI.Endpoints.Cars
{
    public class SearchCarEndpoint : Endpoint<SearchCarRequest>
    {
        private readonly ICarService _carService;
        public SearchCarEndpoint(ICarService carService)
        {
            _carService = carService;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/api/cars/search");
            Validator<SearchCarRequestValidator>();
        }

        public override async Task HandleAsync(SearchCarRequest request, CancellationToken ct)
        {
            try
            {
                var dealerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (dealerId == null)
                {
                    await SendAsync(new { message = "Unauthorized" }, 401);
                    return;
                }

                var cars = await _carService.SearchCarsAsync(int.Parse(dealerId), request.Make, request.Model);

                if (!cars.Any())
                {
                    await SendAsync(new { message = "No cars found" }, 404);
                }
                else
                {
                    await SendOkAsync(cars, ct);
                }
            }
            catch
            {
                await SendAsync(new { message = "An unexpected error occurred." }, 500);
            }
        }
    }
}