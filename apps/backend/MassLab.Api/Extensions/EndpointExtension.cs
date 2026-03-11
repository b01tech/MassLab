using MassLab.Api.Endpoints.ApiHealth;
using MassLab.Api.Endpoints.Identity;
using MassLab.Api.Endpoints.Registry;

namespace MassLab.Api.Extensions;

public static class EndpointExtension
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapApiHealthEndpoint();
        app.MapIdentityEndpoints();
        app.MapRegistryEndpoints();
    }
}

