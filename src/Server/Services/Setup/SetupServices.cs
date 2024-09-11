using BytePlatform.Server.Services.Contracts;
using BytePlatform.Server.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace BytePlatform.Server.Services.Setup;
public static partial class Setup
{
    public static IServiceCollection AddDataContainer<TKey>(this IServiceCollection services) where TKey : Enum
        => services
            .AddScoped<IDataContainerService<TKey>, DataContainerService<TKey>>();
}
