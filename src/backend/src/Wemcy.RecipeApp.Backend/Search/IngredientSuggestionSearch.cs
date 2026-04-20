using Wemcy.RecipeApp.Backend.Model.Entities;

namespace Wemcy.RecipeApp.Backend.Search;

public class IngredientSuggestionSearch(string title) : IQueryFilter<IngredientSuggestion>
{
    private readonly string title = title;
    public string Title => title;

    //this is the logic of how we filter our recipes based on the search query. We use PostgreSQL's full-text search capabilities to match the search terms against the TitleSearchVector of the Recipe entity.
    public IQueryable<IngredientSuggestion> ApplyFilters(IQueryable<IngredientSuggestion> query)
    {
        var queryText = string.Join(" & ", Title
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(term => $"{term}:*"));

        return query
            .Where(x => x.NameSearchVector.Matches(EF.Functions.ToTsQuery(queryText)))
            .OrderByDescending(x => x.NameSearchVector.Rank(EF.Functions.ToTsQuery(queryText)));
    }
}
