using MassLab.Registry.Application.Commands.Client.AddClientContact;
using MassLab.Registry.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MassLab.Api.Endpoints.Registry.UseCases.Client;

public static class AddClientContactUseCase
{
    public record AddClientContactRequest(string Name, string Email, string Phone);

    public static async Task<IResult> ExecuteAsync(
        [FromRoute] Guid id,
        [FromBody] AddClientContactRequest request,
        [FromServices] AddClientContactHandler handler,
        CancellationToken ct)
    {
        var command = new AddClientContactCommand(id, request.Name, request.Email, request.Phone);
        var result = await handler.Handle(command, ct);
        
        return result.IsSuccess
            ? Results.Ok(result.Data) // Usually returns the updated ClientResponse or ContactResponse?
            : Results.BadRequest(result.Errors);
    }
}
