using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Identity.UseCases;

public static class LogoutUseCase
{
    public static IResult Execute(HttpResponse response)
    {
        response.Cookies.Delete("RefreshToken");
        return Results.Ok(new
        {
            message = "Logout successful"
        });
    }
}
