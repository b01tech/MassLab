namespace MassLab.Shared.ValueObject;

public class Pagination(int Page, int PageSize, int TotalItems)
{
    private const int MaxPageSize = 200;
    public int Page { get; } = Page;
    public int PageSize { get; } = PageSize;
    public int TotalItems { get; } = TotalItems;

    public static Pagination Create(int page, int pageSize, int totalItems)
    {
        if (page < 1)
            page = 1;
        if (pageSize < 1)
            pageSize = 25;
        if (pageSize > MaxPageSize)
            pageSize = MaxPageSize;

        return new(page, pageSize, totalItems);
    }

    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
};
