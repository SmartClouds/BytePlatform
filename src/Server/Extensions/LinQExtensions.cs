using System.Linq.Expressions;
using BytePlatform.Server.Models;

namespace System.Collections.Generic;

internal static class LinqExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool predicate, Expression<Func<T, bool>> where) where T : IEntity
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
