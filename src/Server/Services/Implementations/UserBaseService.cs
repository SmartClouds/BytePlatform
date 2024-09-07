using BytePlatform.Server.Data.Contracts;
using BytePlatform.Server.Models;
using BytePlatform.Server.Models.Identity;
using BytePlatform.Shared.Exceptions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BytePlatform.Server.Services.Implementations;

public abstract class UserBaseService<TUserEntity, TEntity, TKey, TStrings> : BaseService<TEntity, TKey, TStrings>
    where TEntity : TUserEntity
    where TUserEntity : UserEntity<TKey>, IEntity
    where TKey : IEquatable<TKey>
{
    #region Fields
    protected readonly ApplicationUserManager<TKey, TUserEntity> UserManager;
    #endregion

    #region Ctor
    protected UserBaseService(IRepository<TEntity, TKey> repository, ILoggerFactory loggerFactory, Type logType, ApplicationUserManager<TKey, TUserEntity> userManager, IStringLocalizer<TStrings> localizer)
        : base(repository, loggerFactory, logType, localizer)
    {
        UserManager = userManager;
    }
    #endregion

    #region CRUD Events

    /// <summary>
    /// Get called right add
    /// </summary>
    protected override async Task ExecuteAddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var password = GetPassword();
        var result = await UserManager.CreateAsync(entity, password).ConfigureAwait(false);

        if (result.Succeeded is false)
        {
            throw new ResourceValidationException(result.Errors.Select(e => Localizer.GetString(e.Code, entity.UserName!)).ToArray());
        }
    }

    /// <summary>
    /// Get called right add
    /// </summary>
    protected override Task ExecuteAddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        throw new InvalidOperationException("Can't add range!");
    }

    /// <summary>
    /// Get called right update
    /// </summary>
    protected override async Task ExecuteEditAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var userEntity = await UserManager.FindByIdAsync<TEntity>(entity.Id).ConfigureAwait(false) ?? throw new ResourceNotFoundException();
        MapEntityOtherEntity(entity, userEntity);
        var result = await UserManager.UpdateAsync(userEntity).ConfigureAwait(false);

        if (result.Succeeded is false)
        {
            throw new ResourceValidationException(result.Errors.Select(e => Localizer.GetString(e.Code, entity.UserName!)).ToArray());
        }
    }

    /// <summary>
    /// Get called right delete
    /// </summary>
    protected override async Task ExecuteRemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var result = await UserManager.DeleteAsync(entity).ConfigureAwait(false);

        if (result.Succeeded is false)
        {
            throw new ResourceValidationException(result.Errors.Select(e => Localizer.GetString(e.Code, entity.UserName!)).ToArray());
        }
    }

    #endregion CRUD Events


    protected abstract string GetPassword();
    protected abstract void MapEntityOtherEntity(TEntity entitySource, TEntity entityDestenition);


    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            UserManager?.Dispose();
        }

        base.Dispose(disposing);
    }
}
