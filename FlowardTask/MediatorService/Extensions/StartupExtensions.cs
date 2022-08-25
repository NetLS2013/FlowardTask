using MediatorService.Middleware;

namespace MediatorService.Extensions
{
    public static class StartupExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddlewareHandler>();
        }
    }
}
