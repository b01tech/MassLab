using MassLab.Shared.ValueObject;

namespace MassLab.Registry.Application.DTOs;

public class PaginatedResponse<T>
{
    public Pagination Pagination { get; }
    public IEnumerable<T> Items { get; }
    public PaginatedResponse(Pagination pagination, IEnumerable<T> items)
    {
        Pagination = pagination;
        Items = items;
    }
}
