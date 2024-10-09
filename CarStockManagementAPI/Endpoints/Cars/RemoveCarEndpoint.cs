using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Services;
using FastEndpoints;

namespace CarStockManagementAPI.Endpoints.Cars
{
    public class RemoveCarEndpoint : Endpoint<RemoveCarRequest, RemoveCarResponse>
    {
        private readonly ICarService _carService;
        public RemoveCarEndpoint(ICarService carService)
        {
            _carService = carService;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/api/cars/remove");
            Validator<RemoveCarRequestValidator>();
        }

        public override async Task HandleAsync(RemoveCarRequest request, CancellationToken ct)
        {
            try
            {
                var dealerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (dealerId == null)
                {
                    await SendAsync(new RemoveCarResponse { Message = "Unauthorized" }, 401);
                    return;
                }

                var result = await _carService.RemoveCarAsync(request.CarId, int.Parse(dealerId));
                if (!result.IsSuccess)
                {
                    await SendAsync(new RemoveCarResponse { Message = result.Message }, 400);
                    return;
                }

                await SendOkAsync(new RemoveCarResponse { Message = result.Message });
            }
            catch
            {
                await SendAsync(new RemoveCarResponse { Message = "An unexpected error occurred." }, 500);
            }
        }
    }
}