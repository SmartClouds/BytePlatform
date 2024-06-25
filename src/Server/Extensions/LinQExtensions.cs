using System.Linq.Expressions;
using BytePlatform.Server.Models;

namespace System.Collections.Generic;

public static class LinqExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool predicate, Expression<Func<T, bool>> where) where T : IEntity
    {
        if (predicate)
            return query.Where(where);

        return query;
    }
}
