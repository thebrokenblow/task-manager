using TaskManager.View.Middlewares;

namespace TaskManager.View.Extensions;

public static class MiddlewareExtension
{
    public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}