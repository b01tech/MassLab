using MassLib.Api.Endpoints.Identity.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace MassLib.Api.Endpoints.Identity;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1")
            .WithTags("Identity");

        group.MapPost("/auth/login", LoginUseCase.ExecuteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Realize login and return token");

        group.MapPost("/users", CreateUserUseCase.ExecuteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new user");

        group.MapPut("/users/{id}", UpdateUserUseCase.ExecuteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Update user details");

        group.MapPatch("/users/{id}/password", ChangePasswordUseCase.ExecuteAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Change user password");

        group.MapPatch("/users/{id}/deactivate", DeactivateUserUseCase.ExecuteAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Deactivate a user");

        group.MapPatch("/users/{id}/activate", ActivateUserUseCase.ExecuteAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Activate a user");
    }
}
