using MassLab.Registry.Application.DTOs;
using MassLab.Registry.Application.Queries.Owner.GetOwner;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Owner;

public static class GetOwnerUseCase
{
    public static async Task<IResult> ExecuteAsync(
        [FromServices] GetOwnerHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetOwnerQuery(), ct);
        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.NotFound(result.Errors);
    }
}
