using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Services;
using FastEndpoints;

namespace CarStockManagementAPI.Endpoints.Cars
{
    public class AddCarEndpoint : Endpoint<AddCarRequest, AddCarResponse>
    {
        private readonly ICarService _carService;
        public AddCarEndpoint(ICarService carService)
        {
            _carService = carService;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/api/cars/add");
            Validator<AddCarRequestValidator>();
        }
        public override async Task HandleAsync(AddCarRequest request, CancellationToken ct)
        {
            var dealerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (dealerId == null)
            {
                await SendAsync(new AddCarResponse { Message = "Unauthorized" }, 401);
                return;
            }
            var result = await _carService.AddCarAsync(request, int.Parse(dealerId));
            if (!result.IsSuccess)
            {
                await SendAsync(new AddCarResponse { Message = result.Message }, 400);
                return;
            }
            await SendOkAsync(new AddCarResponse { Message = result.Message });

        }

    }
}