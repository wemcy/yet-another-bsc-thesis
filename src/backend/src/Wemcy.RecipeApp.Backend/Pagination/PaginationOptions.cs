namespace Wemcy.RecipeApp.Backend.Pagination;

public class PaginationOptions
{
    public int PageNumber { get; }

    public int PageSize { get; }

    public PaginationOptions(int pageNumber, int pageSize)
    {
        ValidatePageSize(pageSize);
        ValidatePageNumber(pageNumber);

        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
    }

    private static void ValidatePageSize(int pageSize)
    {
        if (pageSize <= 0)
        {
            throw new ArgumentException("Page Size must be greater then 0.", nameof(pageSize));
        }
    }

    private static void ValidatePageNumber(int pageNumber)
    {
        if (pageNumber < 0)
        {
            throw new ArgumentException("Page number must be >= 0", nameof(pageNumber));
        }
    }
};
