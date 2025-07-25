namespace BytePlatform.Server.Data.Models;

public interface IEntity
{
}

public interface IEntity<T> : IEntity
{
    T Id { get; set; }
}
