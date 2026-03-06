using MassLib.Api.Endpoints.Identity.UseCases;
using MassLib.Identity.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MassLib.Api.Endpoints.Identity;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1")
            .WithTags("Identity");

        group.MapPost("/login", LoginUseCase.ExecuteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Login");

        group.MapPost("/refresh-token", RefreshTokenUseCase.ExecuteAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Refresh Access Token");

        group.MapPost("/logout", LogoutUseCase.Execute)
            .Produces(StatusCodes.Status200OK)
            .WithSummary("Logout");

        group.MapGet("/users", GetUsersUseCase.ExecuteAsync)
            .Produces<IEnumerable<UserResponse>>(StatusCodes.Status200OK)
            .WithSummary("Get all users");

        group.MapPost("/users", CreateUserUseCase.ExecuteAsync)
            .Produces<UserResponse>(StatusCodes.Status201Created)
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
