using System.Linq.Expressions;

namespace Wemcy.RecipeApp.Backend.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> source, Func<bool> condition, Expression<Func<T, bool>> predicate)
    {
        if (condition())
        {
            return source.Where(predicate);
        }

        return source;
    }
}
