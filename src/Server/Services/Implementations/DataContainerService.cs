using BytePlatform.Server.Services.Contracts;

namespace BytePlatform.Server.Services.Implementations;
public class DataContainerService<TKey> : IDataContainerService<TKey> where TKey : Enum
{
    private readonly IDictionary<TKey, object> keyValues = new Dictionary<TKey, object> { };
    public T Get<T>(TKey key)
    {
        if (keyValues.ContainsKey(key))
        {
            return (T)keyValues[key];
        }
        else
        {
            return default!;
        }
    }
    public void Set<T>(TKey key, T value)
    {
        if (keyValues.ContainsKey(key))
        {
            keyValues[key] = value!;
        }
        else
        {
            keyValues.Add(key, value!);
        }
    }
}
