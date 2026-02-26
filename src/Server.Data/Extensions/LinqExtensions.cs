using System.Linq.Expressions;

namespace BytePlatform.Server.Data.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool predicate, Expression<Func<T, bool>> where)
    {
        if (predicate)
            return query.Where(where);

        return query;
    }

    public static IQueryable<T> SkipIf<T>(this IQueryable<T> query, bool predicate, int count)
    {
        return predicate ? query.Skip(count) : query;
    }

    public static IQueryable<T> TakeIf<T>(this IQueryable<T> query, bool predicate, int count)
    {
        return predicate ? query.Take(count) : query;
    }
}
