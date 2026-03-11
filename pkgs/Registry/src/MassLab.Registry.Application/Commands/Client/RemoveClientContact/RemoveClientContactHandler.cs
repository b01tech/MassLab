using MassLab.Registry.Domain.Interfaces;
using MassLab.Shared.Errors;
using MassLab.Shared.Results;
using MassLab.Shared.ValueObject;

namespace MassLab.Registry.Application.Commands.Client.RemoveClientContact;

public class RemoveClientContactHandler(IClientRepository repository)
{
    public async Task<Result> Handle(RemoveClientContactCommand request, CancellationToken cancellationToken)
    {
        var client = await repository.GetByIdAsync(request.ClientId, cancellationToken);
        if (client == null)
            return Result.Failure(ErrorMessages.CLIENT_NOT_FOUND);

        var contactToRemoveResult = Contact.Create("Unknown", request.Email, request.Phone);
        if (contactToRemoveResult.IsFailure) return Result.Failure(contactToRemoveResult.Errors);

        client.RemoveContact(contactToRemoveResult.Data);

        await repository.UpdateAsync(client, cancellationToken);

        return Result.Success();
    }
}
