using System.Linq.Expressions;
using BytePlatform.Server.Models;

namespace BytePlatform.Server.Extensions;
public static class LinQExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool predicate, Expression<Func<T, bool>> where) where T : IEntity
    {
        if (predicate)
            return query.Where(where);

        return query;
    }
}
