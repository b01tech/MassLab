using MassLib.Api.Endpoints.ApiHealth;
using MassLib.Api.Endpoints.Identity;

namespace MassLib.Api.Extensions;

public static class EndpointExtension
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapApiHealthEndpoint();
        app.MapIdentityEndpoints();
    }
}

