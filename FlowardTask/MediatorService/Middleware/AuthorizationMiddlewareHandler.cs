using MediatorService.Interfaces.IMiddleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using SharedDto.DtoModels.ResponseDtoModels;
using System.Text.Json;

namespace MediatorService.Middleware
{
    public class AuthorizationMiddlewareHandler : IAuthorizationMiddlewareHandler
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddlewareHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Host.Host == "localhost")
            {
                await _next(context);
            }
            else
            {
                BaseResponse response = new BaseResponse()
                {
                    IsSuccess = false,
                    Message = "only localhost allowed!"
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
