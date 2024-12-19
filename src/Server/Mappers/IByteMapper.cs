using BytePlatform.Server.Models;
using BytePlatform.Shared.Dtos;

namespace BytePlatform.Server.Mappers;

public interface IByteMapper<TDto, TEntity>
    where TDto : IDto
    where TEntity : IEntity
{
    abstract IQueryable<TDto> ToDto(IQueryable<TEntity> query);
    abstract TDto ToDto(TEntity entity);
    abstract TEntity ToEntity<T>(T entity);
}
