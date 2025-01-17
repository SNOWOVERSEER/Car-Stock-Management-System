using System;
using System.Threading;
using System.Threading.Tasks;
using CarStockManagementAPI.Dtos;
using CarStockManagementAPI.Services;
using FastEndpoints;

namespace CarStockManagementAPI.Endpoints.Auth
{
    public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
    {
        private readonly IAuthService _authService;
        public LoginEndpoint(IAuthService authService)
        {
            _authService = authService;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/api/auth/login");
            AllowAnonymous();
            Validator<LoginRequestValidator>();
        }
        public override async Task HandleAsync(LoginRequest request, CancellationToken ct)
        {
            try
            {
                var result = await _authService.AuthenticateAsync(request.Email, request.Password);
                if (!result.IsSuccess)
                {
                    await SendAsync(new LoginResponse { Message = result.Message }, 401);
                    return;
                }
                await SendOkAsync(new LoginResponse { Message = result.Message, Token = result.Token });
            }
            catch
            {
                await SendAsync(new LoginResponse { Message = "An unexpected error occurred." }, 500);
            }
        }
    }
}