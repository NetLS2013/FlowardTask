namespace MediatorService.Interfaces.IMiddleware
{
    public interface IAuthorizationMiddlewareHandler
    {
        Task InvokeAsync(HttpContext context);
    }
}
