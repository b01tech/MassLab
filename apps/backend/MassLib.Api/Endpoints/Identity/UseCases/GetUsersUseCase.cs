using MassLib.Identity.Application.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;

namespace MassLib.Api.Endpoints.Identity.UseCases;

public static class GetUsersUseCase
{
    public static async Task<IResult> ExecuteAsync([FromServices] GetUsersHandler handler, CancellationToken ct, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
    {
        var query = new GetUsersQuery(page, pageSize);
        var result = await handler.Handle(query, ct);
        return Results.Ok(result.Data);
    }
}
