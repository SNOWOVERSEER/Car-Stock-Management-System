using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Services;
using FastEndpoints;

namespace CarStockManagementAPI.Endpoints.Auth
{
    [HttpPost("/api/auth/register")]
    public class RegisterEndpoint : Endpoint<RegisterRequest, RegisterResponse>
    {
        private readonly IAuthService _authService;
        public RegisterEndpoint(IAuthService authService)
        {
            _authService = authService;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/api/auth/register");
            AllowAnonymous();
            Validator<RegisterRequestValidator>();
        }
        public override async Task HandleAsync(RegisterRequest request, CancellationToken ct)
        {

            var result = await _authService.RegisterAsync(request.Name, request.Email, request.Password);
            if (!result.IsSuccess)
            {
                await SendAsync(new RegisterResponse { Message = result.Message }, 400);
                return;
            }
            await SendOkAsync(new RegisterResponse { Message = result.Message });
        }
    }
}