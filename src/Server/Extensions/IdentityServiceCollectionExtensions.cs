using BytePlatform.Server.Services.Implementations;
using Microsoft.AspNetCore.Identity;

namespace BytePlatform.Server.Extensions;
public static class IdentityServiceCollectionExtensions
{
    public static IdentityBuilder AddUserManager<TKey, TUser>(this IdentityBuilder identityBuilder) where TUser : class
    {
        return identityBuilder.AddUserManager<ApplicationUserManager<TKey, TUser>>();
    }

    public static IdentityBuilder AddRoleManager<TKey, TRole>(this IdentityBuilder identityBuilder) where TRole : class
    {
        return identityBuilder.AddUserManager<ApplicationRoleManager<TKey, TRole>>();
    }

    public static IdentityBuilder AddUserAndRoleManager<TKey, TUser, TRole>(this IdentityBuilder identityBuilder)
        where TUser : class
        where TRole : class
    {
        return identityBuilder
            .AddUserManager<TKey, TUser>()
            .AddRoleManager<TKey, TRole>();
    }
}
