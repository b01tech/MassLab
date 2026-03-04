namespace MassLib.Api.Endpoints.ApiHealth;

public static class ApiHealthEndpoint
{
    public static void MapApiHealthEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok(new { status = "Ok" }))
            .WithName("ApiHealthCheck")
            .WithTags("API Health")
            .WithSummary("Returns Api Health Status");
    }
}
