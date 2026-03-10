using MassLab.Api.Middlewares;

namespace MassLab.Api.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }

    public static WebApplication UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler();
        return app;
    }
}
