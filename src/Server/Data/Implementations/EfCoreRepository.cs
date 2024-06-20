using System.Linq.Expressions;
using BytePlatform.Server.Data.Contracts;
using BytePlatform.Server.Models;
using BytePlatform.Shared.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BytePlatform.Server.Data.Implementations;

/// <summary>
/// Represents the Entity Framework repository
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public class EfCoreRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity
{
    #region Fields

    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly IDbContext _context;

    private DbSet<TEntity> _entities;

    #endregion

    #region Ctor

    public EfCoreRepository(IDateTimeProvider dateTimeProvider, IDbContext context)
    {
        _dateTimeProvider = dateTimeProvider;
        _context = context;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get entity by identifier
    /// </summary>
    /// <param name="id">Identifier</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Entity</returns>
    public virtual ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        return Entities.FindAsync([id], cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity is IAuditableEntity auditableEntity)
        {
            auditableEntity.CreatedOn = auditableEntity.ModifiedOn = _dateTimeProvider.GetCurrentDateTime();
        }

        if (entity is IArchivableEntity archivableEntity)
        {
            archivableEntity.IsArchived = false;
        }

        Entities.Add(entity);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Insert entities
    /// </summary>
    /// <param name="entities">Entities</param>
    public virtual async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);

        var auditableEntities = entities.OfType<IAuditableEntity>().ToArray();
        foreach (var auditableEntity in auditableEntities)
        {
            auditableEntity.CreatedOn = auditableEntity.ModifiedOn = _dateTimeProvider.GetCurrentDateTime();
        }

        var archivableEntities = entities.OfType<IArchivableEntity>().ToArray();
        foreach (var archivableEntity in archivableEntities)
        {
            archivableEntity.IsArchived = false;
        }

        Entities.AddRange(entities);
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity is IAuditableEntity auditableEntity)
        {
            auditableEntity.ModifiedOn = _dateTimeProvider.GetCurrentDateTime();
        }

        Entities.Update(entity);

        if (entity is IAuditableEntity auditableEntityExc)
        {
            _context.Entry(auditableEntityExc).Property(prop => prop.CreatedOn).IsModified = false;
        }
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, params string[] propertyNameExcludeUpdate)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity is IAuditableEntity auditableEntity)
        {
            auditableEntity.ModifiedOn = _dateTimeProvider.GetCurrentDateTime();
        }

        Entities.Update(entity);

        if (entity is IAuditableEntity auditableEntityExc)
        {
            _context.Entry(auditableEntityExc).Property(prop => prop.CreatedOn).IsModified = false;
        }

        foreach (var item in propertyNameExcludeUpdate)
        {
            _context.Entry(entity).Property(item).IsModified = false;
        }
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] propertyNameExcludeUpdate)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity is IAuditableEntity auditableEntity)
        {
            auditableEntity.ModifiedOn = _dateTimeProvider.GetCurrentDateTime();
        }

        Entities.Update(entity);

        if (entity is IAuditableEntity auditableEntityExc)
        {
            _context.Entry(auditableEntityExc).Property(prop => prop.CreatedOn).IsModified = false;
        }

        foreach (var item in propertyNameExcludeUpdate)
        {
            _context.Entry(entity).Property(item).IsModified = false;
        }
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Update entities
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public virtual async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);

        var auditableEntities = entities.OfType<IAuditableEntity>().ToArray();
        foreach (var auditableEntity in auditableEntities)
        {
            auditableEntity.ModifiedOn = _dateTimeProvider.GetCurrentDateTime();
        }

        Entities.UpdateRange(entities);

        foreach (var auditableEntity in auditableEntities)
        {
            _context.Entry(auditableEntity).Property(nameof(IAuditableEntity.CreatedOn)).IsModified = false;
        }

        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity is IFixedEntity fixedEntity && fixedEntity.IsFixed)
        {
            throw new MemberAccessException(message: "FixedEntitiesMayNotBeDeleted");
        }
        else if (entity is IArchivableEntity archivableEntity)
        {
            archivableEntity.IsArchived = true;
            await UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
            _context.Entry(entity).State = EntityState.Detached;
        }
        else
        {
            Entities.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Delete entities
    /// </summary>
    /// <param name="entities">Entities</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async virtual Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (entities.Any(entity => entity is IFixedEntity fixedEntity && fixedEntity.IsFixed))
        {
            throw new MemberAccessException(message: "FixedEntitiesMayNotBeDeleted");
        }


        var archivableEntities = entities.Where(entity => entity is IArchivableEntity).ToArray();
        if (archivableEntities.Any())
        {
            foreach (var archivableEntity in archivableEntities)
            {
                ((IArchivableEntity)archivableEntity).IsArchived = true;
            }
            await UpdateAsync(archivableEntities, cancellationToken).ConfigureAwait(false);

            foreach (var archivableEntity in archivableEntities)
            {
                _context.Entry(archivableEntities).State = EntityState.Detached;
            }
        }

        if (entities.Any(entity => entity is not IArchivableEntity))
        {
            Entities.RemoveRange(entities.Where(entity => entity is not IArchivableEntity));
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Exists any entity in async by predicate
    /// </summary>
    /// <param name="predicate"></param>
    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return Entities.AnyAsync(predicate, cancellationToken);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a table
    /// </summary>
    public virtual IQueryable<TEntity> Table => Entities;

    /// <summary>
    /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
    /// </summary>
    public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

    /// <summary>
    /// Gets an entity set
    /// </summary>
    protected virtual DbSet<TEntity> Entities
    {
        get
        {
            if (_entities == null)
                _entities = _context.Set<TEntity>();

            return _entities;
        }
    }

    #endregion
}
