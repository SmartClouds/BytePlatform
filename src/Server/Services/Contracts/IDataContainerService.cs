using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytePlatform.Server.Services.Contracts;
public interface IDataContainerService<TKey>
{
    public T Get<T>(TKey key);

    public void Set<T>(TKey key, T value);
}
