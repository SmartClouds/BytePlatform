namespace BytePlatform.Server.Services.Contracts;
public interface IDataContainerService<TKey>
{
    T Get<T>(TKey key);

    void Set<T>(TKey key, T value);
}
