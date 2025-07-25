namespace BytePlatform.Tiny.Shared.Dtos;
public interface IDto
{
}

public interface IDto<T> : IDto
{
    T Id { get; set; }
}
