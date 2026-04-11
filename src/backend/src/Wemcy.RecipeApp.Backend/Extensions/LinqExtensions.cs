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

    public static IQueryable<T> ConditionalWhere<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition)
        {
            return source.Where(predicate);
        }

        return source;
    }

    public static TOutput? MapIfHasElementOrDefault<TInput, TOutput>(this IList<TInput>? source, Func<IList<TInput>, TOutput> mapper, TOutput? defaultValue = default)
    {
        if (source is null)
            return defaultValue;
        if (source.Count == 0)
            return defaultValue;
        return mapper(source);
    }
}
