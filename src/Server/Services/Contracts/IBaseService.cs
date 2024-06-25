using System.ComponentModel.DataAnnotations;
using BytePlatform.Server.Models;

namespace BytePlatform.Server.Services.Contracts;

public interface IBaseService<TEntity, TKey> where TEntity : IEntity
{
    #region Properties

    /// <summary>
    /// Get service result
    /// </summary>
    bool Succeeded { get; }

    /// <summary>
    /// Gets a list of error if service result not succeeded
    /// </summary>
    IList<ValidationResult> Errors { get; }

    #endregion


    #region Methods

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <returns>Entities</returns>
    ValueTask<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Get entity by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <returns>Entity</returns>
    ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);

    /// <summary>
    /// Add new entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Add new entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    ValueTask AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    /// <summary>
    /// Edit new entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    ValueTask EditAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Remove entity
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ValueTask RemoveAsync(TKey key, CancellationToken cancellationToken);

    /// <summary>
    /// Remove entity
    /// </summary>
    /// <param name="entity"></param>
    ValueTask RemoveAsync(TEntity entity, CancellationToken cancellationToken);

    #endregion
}
