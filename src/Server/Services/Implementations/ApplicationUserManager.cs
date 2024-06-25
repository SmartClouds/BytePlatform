using BytePlatform.Server.Models;
using BytePlatform.Shared.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BytePlatform.Server.Services.Implementations;

public class ApplicationUserManager<TKey, TUser> : UserManager<TUser> where TUser : class
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    public ApplicationUserManager(IHttpContextAccessor httpContext,
        IDateTimeProvider dateTimeProvider,
        IUserStore<TUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher,
        IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators,
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
        IServiceProvider services, ILogger<ApplicationUserManager<TKey, TUser>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _httpContext = httpContext;
        _dateTimeProvider = dateTimeProvider;
    }

    protected override CancellationToken CancellationToken => _httpContext?.HttpContext?.RequestAborted ?? base.CancellationToken;

    public virtual async Task<bool> ExistsUserNameAsync(string userName)
    {
        ThrowIfDisposed();
        ArgumentException.ThrowIfNullOrEmpty(userName, nameof(userName));

        userName = NormalizeName(userName);

        var user = await Store.FindByNameAsync(userName, CancellationToken).ConfigureAwait(false);

        return user is not null;
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.
    /// </returns>
    public virtual Task<TUser?> FindByIdAsync(TKey userId)
    {
        return FindByIdAsync(userId?.ToString()!);
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.
    /// </returns>
    public virtual async Task<T?> FindByIdAsync<T>(TKey userId)
    {
        return (T?)Convert.ChangeType(await FindByIdAsync(userId), typeof(T));
    }

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the backing store with no password,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
    /// of the operation.
    /// </returns>
    public override async Task<IdentityResult> CreateAsync(TUser user)
    {
        ThrowIfDisposed();

        if (user is IAuditableEntity auditableEntity)
        {
            auditableEntity.CreatedOn = auditableEntity.ModifiedOn = _dateTimeProvider.GetCurrentDateTime();
        }

        if (user is IArchivableEntity archivableEntity)
        {
            archivableEntity.IsArchived = false;
        }

        return await base.CreateAsync(user).ConfigureAwait(false);
    }

    protected override async Task<IdentityResult> UpdateUserAsync(TUser user)
    {
        if (user is IFixedEntity fixedEntity && fixedEntity.IsFixed)
        {
            throw new MemberAccessException(message: "FixedEntitiesMayNotBeUpdated");
        }

        if (user is IAuditableEntity auditableEntity)
        {
            if (auditableEntity.CreatedOn == default)
            {
                throw new InvalidDataException(nameof(auditableEntity.CreatedOn));
            }

            auditableEntity.ModifiedOn = _dateTimeProvider.GetCurrentDateTime();
        }

        return await base.UpdateUserAsync(user).ConfigureAwait(false);
    }

    public override async Task<IdentityResult> DeleteAsync(TUser user)
    {
        if (user is IFixedEntity fixedEntity && fixedEntity.IsFixed)
        {
            throw new MemberAccessException(message: "FixedEntitiesMayNotBeDeleted");
        }
        else if (user is IArchivableEntity archivableEntity)
        {
            archivableEntity.IsArchived = true;
            return await base.UpdateUserAsync(user).ConfigureAwait(false);
        }
        else
        {
            return await base.DeleteAsync(user).ConfigureAwait(false);
        }
    }
}
