using BytePlatform.Server.Data.Contracts;
using BytePlatform.Server.Data.Implementations;
using BytePlatform.Server.Mappers;
using BytePlatform.Server.Models.Identity;
using BytePlatform.Server.Services.Contracts;
using BytePlatform.Server.Services.Implementations;
using BytePlatform.Server.Services.Setup;
using BytePlatform.Shared.Services;
using BytePlatform.Shared.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace BytePlatform.Server.Infrastructure.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddServerServices<TAssembly, TDbContext, TKey, TUser, TRole, TDataContainerKey>(this IServiceCollection services)
        where TDbContext : ApplicationDbContext<TKey, TUser, TRole>
        where TKey : IEquatable<TKey>
        where TUser : UserEntity<TKey>
        where TRole : RoleEntity<TKey>
        where TDataContainerKey : Enum
    {
        return AddServerServices<TAssembly, TDbContext, TKey, TUser, TRole, UserClaimEntity<TKey>, UserRoleEntity<TKey>, UserLoginEntity<TKey>, RoleClaimEntity<TKey>, UserTokenEntity<TKey>, TDataContainerKey>(services);
    }

    public static IServiceCollection AddServerServices<TAssembly, TDbContext, TKey, TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TDataContainerKey>(this IServiceCollection services)
        where TDbContext : ApplicationDbContext<TKey, TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TKey : IEquatable<TKey>
        where TUser : UserEntity<TKey>
        where TRole : RoleEntity<TKey>
        where TUserClaim : UserClaimEntity<TKey>
        where TUserRole : UserRoleEntity<TKey>
        where TUserLogin : UserLoginEntity<TKey>
        where TRoleClaim : RoleClaimEntity<TKey>
        where TUserToken : UserTokenEntity<TKey>
        where TDataContainerKey : Enum
    {
        // dbContext
        services.AddScoped(typeof(IDbContext), typeof(TDbContext));

        // date time provider
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        // repositories
        services.AddScoped(typeof(IRepository<,>), typeof(EfCoreRepository<,>));

        // services
        services.Scan(scan =>
        {
            scan
                .FromAssemblyOf<TAssembly>()
                .AddClasses(classes => classes.AssignableTo(typeof(IBaseService<,>)))
                .AsMatchingInterface()
                .WithScopedLifetime();
        });

        // mapper
        services.Scan(scan =>
        {
            scan
                .FromAssemblyOf<TAssembly>()
                .AddClasses(classes => classes.AssignableTo(typeof(IByteMapper<,>)))
                .AsMatchingInterface()
                .WithTransientLifetime();
        });

        services.AddScoped(typeof(IUserInformationProvider<>), typeof(UserInformationProvider<>));

        services.AddDataContainer<TDataContainerKey>();

        return services;
    }
}
