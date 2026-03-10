namespace MassLab.Registry.Application.Queries.Client.GetClients;

public record GetClientsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null
);
