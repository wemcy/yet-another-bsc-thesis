using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Search;

public class RecipeSearch(string title) : IQueryFilter<Recipe>
{
    private readonly string title = title;
    public string Title => title;

    public IQueryable<Recipe> ApplyFilters(IQueryable<Recipe> query)
    {
        var queryText = string.Join(" & ", Title
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(term => $"{term}:*"));

        return query
            .Where(x => x.TitleSearchVector.Matches(EF.Functions.ToTsQuery(queryText)))
            .OrderByDescending(x => x.TitleSearchVector.Rank(EF.Functions.ToTsQuery(queryText)));
    }
}
