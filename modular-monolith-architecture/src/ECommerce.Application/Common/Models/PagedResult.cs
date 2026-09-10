namespace ECommerce.Application.Common.Models;

public sealed record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount)
{
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
}

public abstract record PaginationRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;

    public void Validate()
    {
        if (PageNumber < 1) throw new ArgumentOutOfRangeException(nameof(PageNumber));
        if (PageSize is < 1 or > 100) throw new ArgumentOutOfRangeException(nameof(PageSize));
    }
}
