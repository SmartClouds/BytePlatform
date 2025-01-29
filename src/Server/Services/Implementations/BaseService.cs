using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using BytePlatform.Server.Data.Contracts;
using BytePlatform.Server.Models;
using BytePlatform.Server.Services.Contracts;
using BytePlatform.Server.Services.Helpers;
using BytePlatform.Shared.Exceptions;
using BytePlatform.Shared.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BytePlatform.Server.Services.Implementations;

public abstract class BaseService<TEntity, TKey, TStrings> : IBaseService<TEntity, TKey>, IDisposable where TEntity : IEntity
{
    #region Fields
    private bool _disposed;
    #endregion

    #region Properties
    protected IRepository<TEntity, TKey> Repository { get; init; }
    protected ILogger Logger { get; init; }
    protected IStringLocalizer<TStrings> Localizer { get; init; }

    protected ServiceResult ServiceResult { get; init; }

    protected EntityValidationResult ValidationResult
    {
        get
        {
            return ServiceResult.ValidationResult;
        }
        set
        {
            ServiceResult.ValidationResult = value;
        }
    }

    public bool Succeeded => !ValidationResult.Errors.Any();
    public IList<ValidationResult> Errors => ValidationResult.Errors;


    protected virtual string? GetClaim => null;
    protected virtual string? AddClaim => null;
    protected virtual string? EditClaim => null;
    protected virtual string? RemoveClaim => null;
    #endregion

    #region Ctor
    protected BaseService(IRepository<TEntity, TKey> repository, ILoggerFactory loggerFactory, Type logType, IStringLocalizer<TStrings> localizer)
    {
        Logger = loggerFactory.CreateLogger(logType);
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        ServiceResult = new ServiceResult();
        Localizer = localizer;
    }
    #endregion

    #region Methods

    public virtual async ValueTask<IQueryable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        if (await EnsureValidationAndAccessAsync(OperationKind.Get, cancellationToken).ConfigureAwait(false))
        {
            var predicate = await GetFilterAsync(cancellationToken).ConfigureAwait(false);
            return Repository.TableNoTracking.WhereIf(predicate is not null, predicate!);
        }
        else
        {
            throw new UnauthorizedAccessException();
        }
    }

    public virtual async ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
            return entity;

        if (await EnsureValidationAndAccessAsync(OperationKind.Get, entity, cancellationToken).ConfigureAwait(false))
            return entity;

        return default;
    }

    public virtual async ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (await EnsureValidationAndAccessAsync(OperationKind.Add, entity, cancellationToken).ConfigureAwait(false))
        {
            await OnAddingAsync(entity, cancellationToken).ConfigureAwait(false);
            await ExecuteAddAsync(entity, cancellationToken).ConfigureAwait(false);
            await OnAddedAsync(entity, cancellationToken).ConfigureAwait(false);
        }
    }

    public virtual async ValueTask AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
        {
            if (await EnsureValidationAndAccessAsync(OperationKind.Add, entity, cancellationToken).ConfigureAwait(false) is false)
                return;
        }

        await OnAddingRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        await ExecuteAddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        await OnAddedRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public virtual async ValueTask EditAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (await EnsureValidationAndAccessAsync(OperationKind.Edit, entity, cancellationToken).ConfigureAwait(false))
        {
            await OnEditingAsync(entity, cancellationToken).ConfigureAwait(false);
            await ExecuteEditAsync(entity, cancellationToken).ConfigureAwait(false);
            await OnEditedAsync(entity, cancellationToken).ConfigureAwait(false);
        }
    }

    public virtual async ValueTask RemoveAsync(TKey key, string concurrencyStamp, CancellationToken cancellationToken)
    {
        var entity = await Repository.GetByIdAsync(key, cancellationToken);
        if (entity is not null)
        {
            if (await EnsureValidationAndAccessAsync(OperationKind.Remove, entity, cancellationToken).ConfigureAwait(false) is false)
                return;

            if (entity is IRowVersionedEntity rowVersioned && rowVersioned.ConcurrencyStamp.SequenceEqual(Convert.FromBase64String(Uri.UnescapeDataString(concurrencyStamp))) is false)
                throw new ConflictException(BytePlatformStrings.General.UpdateConcurrencyException);

            await OnRemovingAsync(entity, cancellationToken).ConfigureAwait(false);
            await ExecuteRemoveAsync(entity, cancellationToken).ConfigureAwait(false);
            await OnRemovedAsync(entity, cancellationToken).ConfigureAwait(false);
        }
    }

    public virtual async ValueTask RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        if (entity is not null)
        {
            if (await EnsureValidationAndAccessAsync(OperationKind.Remove, entity, cancellationToken).ConfigureAwait(false) is false)
                return;

            await OnRemovingAsync(entity, cancellationToken).ConfigureAwait(false);
            await ExecuteRemoveAsync(entity, cancellationToken).ConfigureAwait(false);
            await OnRemovedAsync(entity, cancellationToken).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #region Protected Methods

    //protected virtual async ValueTask<TModel> FindAsync<TModel>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) where TModel : class
    //{
    //    return _repository.TableNoTracking.Where(predicate)
    //        .ToModel<TModel>()
    //        .FirstOrDefaultAsync(cancellationToken);
    //}
    //protected virtual async ValueTask<IReadOnlyList<TModel>> GetListAsync<TModel>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy = null, bool orderByDescending = false) where TModel : class
    //{
    //    IQueryable<TEntity> query = _repository.TableNoTracking.Where(predicate);

    //    if (orderBy != null)
    //    {
    //        if (orderByDescending)
    //        {
    //            query = query.OrderByDescending(orderBy);
    //        }
    //        else
    //        {
    //            query = query.OrderBy(orderBy);
    //        }
    //    }

    //    return await query
    //        .ToModel<TModel>()
    //        .ToListAsync();
    //}
    //protected virtual async ValueTask EditAsync(TEntity entity, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] propertyNameExcloudUpdate)
    //{
    //    if (this.ModelStateIsValid(entity))
    //    {
    //        await _repository.UpdateAsync(entity, propertyNameExcloudUpdate);
    //    }
    //}

    //protected virtual async ValueTask BatchRemoveAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    //{
    //    await _repository.Table.Where(predicate).BatchDeleteAsync();
    //}

    //protected virtual async ValueTask BatchEditAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression, CancellationToken cancellationToken)
    //{
    //    await _repository.Table.Where(predicate).BatchUpdateAsync(updateExpression);
    //}

    //protected virtual async ValueTask BatchEditAsync(Expression<Func<TEntity, bool>> predicate, object updateValues, List<string> updateColumns = null)
    //{
    //    await _repository.Table.Where(predicate).BatchUpdateAsync(updateValues, updateColumns);
    //}

    //protected virtual async ValueTask BatchEditAsync(Expression<Func<TEntity, bool>> predicate, object updateValues, string updateColumn, CancellationToken cancellationToken)
    //{
    //    if (string.IsNullOrEmpty(updateColumn))
    //    {
    //        await this.BatchEditAsync(predicate, updateValues);
    //    }
    //    else
    //    {
    //        await this.BatchEditAsync(predicate, updateValues, new List<string> { updateColumn });
    //    }
    //}

    protected async ValueTask<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await EnsureValidationAndAccessAsync(OperationKind.Get, cancellationToken).ConfigureAwait(false)
                && await Repository.ExistsAsync(predicate, cancellationToken).ConfigureAwait(false);
    }

    #region CRUD Events

    /// <summary>
    /// Gets called right before Add
    /// </summary>
    protected virtual ValueTask OnAddingAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Get called right add
    /// </summary>
    protected virtual Task ExecuteAddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Repository.InsertAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Gets called right after Add
    /// </summary>
    protected virtual ValueTask OnAddedAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Gets called right before Add
    /// </summary>
    protected virtual ValueTask OnAddingRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Get called right add
    /// </summary>
    protected virtual Task ExecuteAddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return Repository.InsertAsync(entities, cancellationToken);
    }

    /// <summary>
    /// Gets called right after Add
    /// </summary>
    protected virtual ValueTask OnAddedRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Gets called right before update
    /// </summary>
    protected virtual ValueTask OnEditingAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Get called right update
    /// </summary>
    protected virtual Task ExecuteEditAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Repository.UpdateAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Gets called right after update
    /// </summary>
    protected virtual ValueTask OnEditedAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
    /// <summary>
    /// Gets called right before delete
    /// </summary>
    protected virtual ValueTask OnRemovingAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Get called right delete
    /// </summary>
    protected virtual Task ExecuteRemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Repository.DeleteAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Gets called right after delete
    /// </summary>
    protected virtual ValueTask OnRemovedAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (!disposing) return;

        //_repository.Dispose();

        _disposed = true;
    }

    #endregion CRUD Events

    #endregion Protected Methods

    #endregion

    #region Utitlties

    protected virtual ValueTask<bool> EnsureValidationAndAccessAsync(OperationKind kind, CancellationToken cancellationToken)
        => EnsureValidationAndAccessAsync(kind, default, cancellationToken);
    protected virtual async ValueTask<bool> EnsureValidationAndAccessAsync(OperationKind kind, TEntity? entity, CancellationToken cancellationToken)
    {
        if (kind == OperationKind.Add || kind == OperationKind.Edit)
        {
            if (entity is null || await EnsureValidationAsync(entity, cancellationToken) is false)
            {
                return false;
            }
        }

        return await EnsureCrudClaimsAsync(kind, entity, cancellationToken).ConfigureAwait(false);
    }

    protected virtual async ValueTask<bool> EnsureValidationAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ValidationResult = DataAnnotation.ValidateEntity(entity);

        return ValidationResult.IsValid;
    }

    protected virtual async ValueTask<bool> EnsureCrudClaimsAsync(OperationKind kind, TEntity? entity, CancellationToken cancellationToken)
    {
        var claim = kind switch
        {
            OperationKind.Add => AddClaim,
            OperationKind.Remove => RemoveClaim,
            OperationKind.Edit => EditClaim,
            OperationKind.Get => GetClaim,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };

        if (string.IsNullOrEmpty(claim))
        {
            return true;
        }

        return await EnsureClaimAsync(claim, cancellationToken).ConfigureAwait(false);
    }

    protected virtual async ValueTask<bool> EnsureClaimAsync(string claim, CancellationToken cancellationToken) => true;

    protected virtual async ValueTask<Expression<Func<TEntity, bool>>?> GetFilterAsync(CancellationToken cancellationToken) => null;

    #endregion
}
