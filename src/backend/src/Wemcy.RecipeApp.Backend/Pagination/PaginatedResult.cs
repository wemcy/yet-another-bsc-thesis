namespace Wemcy.RecipeApp.Backend.Pagination;

public class PaginatedResult<T> : IAsyncEnumerable<T>, IPaginatedResult
{
    private readonly int _pageSize;
    private readonly int _totalCount;
    private readonly int _pageNumber;

    private readonly IAsyncEnumerable<T> _items;

    public int TotalCount => _totalCount;

    public int PageSize => _pageSize;

    public int PageNumber => _pageNumber;

    public int PageCount => (int)Math.Ceiling((float)TotalCount / PageSize);

    public bool HasNextPage => PageNumber < PageCount - 1;

    public bool HasPreviousPage => _pageNumber > 0;


    private PaginatedResult(IAsyncEnumerable<T> items, int total, int pageSize, int pageNumber)
    {
        this._items = items;
        this._totalCount = total;
        this._pageSize = pageSize;
        this._pageNumber = pageNumber;
    }

    public static async Task<PaginatedResult<T>> CreateAsync(IQueryable<T> source, PaginationOptions options)
    {
        var totalCount = await source.CountAsync();
        var items = source
            .Skip(options.PageNumber * options.PageSize)
            .Take(options.PageSize)
            .AsAsyncEnumerable();

        return new PaginatedResult<T>(items, totalCount, options.PageSize, options.PageNumber);
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return _items.GetAsyncEnumerator(cancellationToken);
    }
}
