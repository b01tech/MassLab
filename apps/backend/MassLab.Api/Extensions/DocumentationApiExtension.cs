namespace MassLab.Api.Extensions;

public static class DocumentationApiExtension
{
    public static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }

    public static void UseDocumentation(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return;

        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "docs";
            options.DocumentTitle = "MassLab API";
            options.SwaggerEndpoint("/openapi/v1.json", "MassLab API v1");
        });
    }

}
