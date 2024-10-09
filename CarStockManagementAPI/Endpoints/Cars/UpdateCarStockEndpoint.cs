using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Services;
using FastEndpoints;

namespace CarStockManagementAPI.Endpoints.Cars
{
    public class UpdateCarStockEndpoint : Endpoint<UpdateCarStockRequest, UpdateCarStockResponse>
    {
        private readonly ICarService _carService;
        public UpdateCarStockEndpoint(ICarService carService)
        {
            _carService = carService;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("api/cars/update-stock");
            Validator<UpdateCarStockRequestValidator>();
        }

        public override async Task HandleAsync(UpdateCarStockRequest request, CancellationToken ct)
        {
            try
            {
                var dealerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (dealerId == null)
                {
                    await SendAsync(new UpdateCarStockResponse { Message = "Unauthorized" }, 401);
                    return;
                }

                var result = await _carService.UpdateCarStockAsync(int.Parse(dealerId), request.CarId, request.NewStock);

                if (!result.IsSuccess)
                {
                    await SendAsync(new UpdateCarStockResponse { Message = result.Message }, 400);
                    return;
                }

                await SendOkAsync(new UpdateCarStockResponse { Message = result.Message });
            }
            catch
            {
                await SendAsync(new UpdateCarStockResponse { Message = "An unexpected error occurred." }, 500);
            }
        }
    }
}