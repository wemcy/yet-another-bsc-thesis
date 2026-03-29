public interface IPaginatedResult
{
    bool HasNextPage { get; }
    bool HasPreviousPage { get; }
    int PageCount { get; }
    int PageNumber { get; }
    int PageSize { get; }
    int TotalCount { get; }
}
