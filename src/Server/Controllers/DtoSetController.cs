using BytePlatform.Server.Models;
using BytePlatform.Server.Services.Contracts;
using BytePlatform.Shared.Dtos;
using BytePlatform.Shared.Exceptions;
using BytePlatform.Shared.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BytePlatform.Server.Controllers;

public abstract partial class DtoSetController<TKey, TDto, TEntity, TResourceSettings> : DtoSetController<TKey, TDto, TEntity, IBaseService<TEntity, TKey>, TResourceSettings>
    where TDto : IDto<TKey>
    where TEntity : IEntity
{
    protected DtoSetController(IBaseService<TEntity, TKey> entityService, IStringLocalizer<TResourceSettings> localizer, IUserInformationProvider<TKey> userInformationProvider)
        : base(entityService, localizer, userInformationProvider)
    {
    }
}

public abstract partial class DtoSetController<TKey, TDto, TEntity, TService, TResourceSettings> : DtoSetController<TKey, TDto, TDto, TDto, TEntity, TService, TResourceSettings>
    where TDto : IDto<TKey>
    where TEntity : IEntity
    where TService : IBaseService<TEntity, TKey>
{
    protected DtoSetController(TService entityService, IStringLocalizer<TResourceSettings> localizer, IUserInformationProvider<TKey> userInformationProvider)
        : base(entityService, localizer, userInformationProvider)
    {
    }
}

[Route("api/[controller]/[action]")]
[ApiController]
public abstract partial class DtoSetController<TKey, TDto, TDtoAdd, TDtoEdit, TEntity, TService, TResourceSettings> : ControllerBase
    where TDto : IDto<TKey>
    where TDtoAdd : IDto<TKey>
    where TDtoEdit : IDto<TKey>
    where TEntity : IEntity
    where TService : IBaseService<TEntity, TKey>
{
    protected readonly TService EntityService;
    protected readonly IStringLocalizer<TResourceSettings> Localizer;
    protected readonly IUserInformationProvider<TKey> UserInformationProvider;

    protected DtoSetController(TService entityService, IStringLocalizer<TResourceSettings> localizer, IUserInformationProvider<TKey> userInformationProvider)
    {
        EntityService = entityService;
        Localizer = localizer;
        UserInformationProvider = userInformationProvider;
    }

    [HttpGet, EnableQuery]
    public virtual async Task<IQueryable<TDto>> Get(CancellationToken cancellationToken)
    {
        var query = await EntityService.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return ToDto(query);
    }

    [HttpGet("{id:guid}")]
    public async Task<TDto> Get(TKey id, CancellationToken cancellationToken)
    {
        var entity = await EntityService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
            throw new ResourceNotFoundException(Localizer[BytePlatformStrings.General.ItemCouldNotBeFound]);

        return ToDto(entity);
    }

    [HttpGet]
    public virtual async Task<PagedResult<TDto>> GetPage(ODataQueryOptions<TDto> options, CancellationToken cancellationToken)
    {
        IQueryable query = options.ApplyTo(await Get(cancellationToken).ConfigureAwait(false), ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.Select);
        if (query is IQueryable<TDto> resultQuery)
        {
            var totalCount = await resultQuery.LongCountAsync(cancellationToken).ConfigureAwait(false);

            if (options.Skip is not null)
                resultQuery = resultQuery.Skip(options.Skip.Value);

            if (options.Top is not null)
                resultQuery = resultQuery.Take(options.Top.Value);

            return new PagedResult<TDto>(await resultQuery.ToArrayAsync(cancellationToken).ConfigureAwait(false), totalCount);
        }

        throw new NotSupportedException();
    }

    [HttpPost]
    public virtual async Task<TDto> Create(TDtoAdd dto, CancellationToken cancellationToken)
    {
        var entity = ToEntity<TDtoAdd>(dto);

        await EntityService.AddAsync(entity, cancellationToken).ConfigureAwait(false);

        return ToDto(entity);
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<TDto> Update(TKey id, TDtoEdit dto, CancellationToken cancellationToken)
    {
        if (EqualityComparer<TKey>.Default.Equals(id, dto.Id) is false)
            throw new ResourceNotFoundException(Localizer[BytePlatformStrings.General.ItemCouldNotBeFound]);

        var entity = ToEntity<TDtoEdit>(dto);

        await EntityService.EditAsync(entity, cancellationToken).ConfigureAwait(false);

        return ToDto(entity);
    }

    [HttpDelete("{id:guid}")]
    public virtual async Task Delete(TKey id, CancellationToken cancellationToken)
    {
        await EntityService.RemoveAsync(id, cancellationToken).ConfigureAwait(false);
    }



    protected abstract IQueryable<TDto> ToDto(IQueryable<TEntity> query);
    protected abstract TDto ToDto(TEntity entity);
    protected abstract TEntity ToEntity<T>(T entity);
}
