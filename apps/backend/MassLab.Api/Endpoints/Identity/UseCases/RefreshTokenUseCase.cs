using MassLab.Identity.Application.Commands.RefreshToken;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Identity.UseCases;

public static class RefreshTokenUseCase
{
    public static async Task<IResult> ExecuteAsync(HttpContext context, [FromServices] RefreshTokenHandler handler, [FromServices] IConfiguration configuration, CancellationToken ct)
    {
        if (!context.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
        {
            return Results.Unauthorized();
        }

        var command = new RefreshTokenCommand(refreshToken);
        var result = await handler.Handle(command, ct);

        if (result.IsFailure)
        {
            context.Response.Cookies.Delete("AccessToken");
            context.Response.Cookies.Delete("RefreshToken");
            return Results.Unauthorized();
        }

        var tokenResponse = result.Data;

        var refreshTokenExpiry = double.Parse(configuration["Jwt:RefreshExpirationInDays"] ?? "7");

        context.Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(refreshTokenExpiry)
        });

        return Results.Ok(new
        {
            accessToken = tokenResponse.AccessToken,
        });
    }
}
