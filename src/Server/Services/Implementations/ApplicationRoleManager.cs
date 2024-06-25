using BytePlatform.Server.Models;
using BytePlatform.Shared.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BytePlatform.Server.Services.Implementations;

public class ApplicationRoleManager<TKey, TRole> : RoleManager<TRole> where TRole : class
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    public ApplicationRoleManager(IRoleStore<TRole> store,
        IEnumerable<IRoleValidator<TRole>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<ApplicationRoleManager<TKey, TRole>> logger,
        IHttpContextAccessor httpContext,
        IDateTimeProvider dateTimeProvider)
        : base(store, roleValidators, keyNormalizer, errors, logger)
    {
        _httpContext = httpContext;
        _dateTimeProvider = dateTimeProvider;
    }

    protected override CancellationToken CancellationToken => _httpContext?.HttpContext?.RequestAborted ?? base.CancellationToken;

    public override async Task<IdentityResult> UpdateAsync(TRole role)
    {
        if (role is IFixedEntity fixedEntity && fixedEntity.IsFixed)
        {
            throw new MemberAccessException(message: "FixedEntitiesMayNotBeUpdated");
        }

        if (role is IAuditableEntity auditableEntity)
        {
            if (auditableEntity.CreatedOn == default)
            {
                throw new InvalidDataException(nameof(auditableEntity.CreatedOn));
            }

            auditableEntity.ModifiedOn = _dateTimeProvider.GetCurrentDateTime();
        }

        if (role is IArchivableEntity archivableEntity)
        {
            archivableEntity.IsArchived = false;
        }

        return await base.UpdateAsync(role).ConfigureAwait(false);
    }

    public override async Task<IdentityResult> DeleteAsync(TRole role)
    {
        if (role is IFixedEntity fixedEntity && fixedEntity.IsFixed)
        {
            throw new MemberAccessException(message: "FixedEntitiesMayNotBeDeleted");
        }
        else if (role is IArchivableEntity archivableEntity)
        {
            archivableEntity.IsArchived = true;
            return await UpdateAsync(role).ConfigureAwait(false);
        }
        else
        {
            return await base.DeleteAsync(role);
        }
    }
}
