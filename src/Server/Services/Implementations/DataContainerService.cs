using BytePlatform.Server.Services.Contracts;

namespace BytePlatform.Server.Services.Implementations;
public class DataContainerService<TKey> : IDataContainerService<TKey> where TKey : Enum
{
    private readonly IDictionary<TKey, object> _keyValues = new Dictionary<TKey, object>();
    public T Get<T>(TKey key)
    {
        if (_keyValues.ContainsKey(key))
        {
            return (T)_keyValues[key];
        }
        else
        {
            return default!;
        }
    }
    public void Set<T>(TKey key, T value)
    {
        if (_keyValues.ContainsKey(key))
        {
            _keyValues[key] = value!;
        }
        else
        {
            _keyValues.Add(key, value!);
        }
    }
}
