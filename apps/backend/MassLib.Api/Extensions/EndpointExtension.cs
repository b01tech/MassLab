using MassLib.Api.Endpoints.ApiHealth;

namespace MassLib.Api.Extensions;

public static class EndpointExtension
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapApiHealthEndpoint();
    }
}
