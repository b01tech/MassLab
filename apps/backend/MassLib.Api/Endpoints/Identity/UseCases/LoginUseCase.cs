using MassLib.Identity.Application.Commands.Login;
using Microsoft.AspNetCore.Mvc;

namespace MassLib.Api.Endpoints.Identity.UseCases;

public static class LoginUseCase
{
    public static async Task<IResult> ExecuteAsync([FromBody] LoginCommand command, [FromServices] LoginHandler handler, HttpContext context, [FromServices] IConfiguration configuration,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);

        if (result.IsFailure)
            return Results.BadRequest(result.Errors);

        var tokenResponse = result.Data;

        var refreshTokenExpiry = double.Parse(configuration["Jwt:RefreshExpirationInDays"] ?? "7");

        context.Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(refreshTokenExpiry)
        });

        // Retorna Access Token no corpo para o Frontend armazenar em memória
        return Results.Ok(new
        {
            accessToken = tokenResponse.AccessToken,
        });
    }
}
