using BytePlatform.Server.Data.Contracts;
using BytePlatform.Server.Data.Implementations;
using BytePlatform.Server.Models.Identity;
using BytePlatform.Server.Services.Contracts;
using BytePlatform.Server.Services.Implementations;
using BytePlatform.Shared.Services;
using BytePlatform.Shared.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace BytePlatform.Server.Infrastructure.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddServerServices<TAssembly, TDbContext, TKey, TUser, TRole>(this IServiceCollection services)
        where TDbContext : ApplicationDbContext<TKey, TUser, TRole>
        where TKey : IEquatable<TKey>
        where TUser : UserEntity<TKey>
        where TRole : RoleEntity<TKey>
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

        services.AddScoped(typeof(IUserInformationProvider<>), typeof(UserInformationProvider<>));

        return services;
    }
}
