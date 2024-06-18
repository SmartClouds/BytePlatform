using System.Linq.Expressions;
using BytePlatform.Server.Models;

namespace BytePlatform.Server.Data.Contracts;

/// <summary>
/// Represents an entity repository
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public partial interface IRepository<TEntity, TKey> where TEntity : IEntity
{
    #region Methods

    /// <summary>
    /// Get entity by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Entity</returns>
    ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Insert entities
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    /// <summary>
    /// Update entity excloud properties
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <param name="propertyNameExcloudUpdate"></param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, params string[] propertyNameExcloudUpdate);

    /// <summary>
    /// Update entity excloud properties
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <param name="propertyNameExcloudUpdate"></param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] propertyNameExcloudUpdate);

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    /// <summary>
    /// Exists any entity in async by predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    #endregion

    #region Properties

    /// <summary>
    /// Gets a table
    /// </summary>
    IQueryable<TEntity> Table { get; }

    /// <summary>
    /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
    /// </summary>
    IQueryable<TEntity> TableNoTracking { get; }

    #endregion
}
