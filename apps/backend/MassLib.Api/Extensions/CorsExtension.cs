namespace MassLib.Api.Extensions;

public static class CorsExtension
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }

    public static WebApplication UseCorsPolicy(this WebApplication app)
    {
        app.UseCors();
        return app;
    }
}
