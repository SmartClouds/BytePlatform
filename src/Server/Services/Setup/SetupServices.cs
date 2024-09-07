using BytePlatform.Server.Services.Contracts;
using BytePlatform.Server.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace BytePlatform.Server.Services.Setup;
public static partial class Setup
{
    public static IServiceCollection AddDataContainer<TKey>(this IServiceCollection services) where TKey : Enum
        => services
            .AddSingleton<IDataContainerService<TKey>, DataContainerService<TKey>>();


    public static IServiceCollection AddDataContainer<TKey, TDataContainerService>(this IServiceCollection services) where TKey : Enum
        => services
            .AddSingleton(typeof(TDataContainerService), typeof(DataContainerService<TKey>));
}
