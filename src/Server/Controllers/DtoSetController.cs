using BytePlatform.Server.Mappers;
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

public abstract partial class DtoSetController<TKey, TDto, TEntity, TMapper, TResourceSettings> : DtoSetController<TKey, TDto, TEntity, IBaseService<TEntity, TKey>, TMapper, TResourceSettings>
    where TDto : IDto<TKey>
    where TEntity : IEntity
    where TMapper : IByteMapper<TDto, TEntity>
{
    protected DtoSetController(IBaseService<TEntity, TKey> entityService, TMapper mapper, IStringLocalizer<TResourceSettings> localizer, IUserInformationProvider<TKey> userInformationProvider)
        : base(entityService, mapper, localizer, userInformationProvider)
    {
    }
}

public abstract partial class DtoSetController<TKey, TDto, TEntity, TService, TMapper, TResourceSettings> : DtoSetController<TKey, TDto, TDto, TDto, TEntity, TService, TMapper, TResourceSettings>
    where TDto : IDto<TKey>
    where TEntity : IEntity
    where TService : IBaseService<TEntity, TKey>
    where TMapper : IByteMapper<TDto, TEntity>
{
    protected DtoSetController(TService entityService, TMapper mapper, IStringLocalizer<TResourceSettings> localizer, IUserInformationProvider<TKey> userInformationProvider)
        : base(entityService, mapper, localizer, userInformationProvider)
    {
    }
}

[Route("api/[controller]/[action]")]
[ApiController]
public abstract partial class DtoSetController<TKey, TDto, TDtoAdd, TDtoEdit, TEntity, TService, TMapper, TResourceSettings> : ControllerBase
    where TDto : IDto<TKey>
    where TDtoAdd : IDto<TKey>
    where TDtoEdit : IDto<TKey>
    where TEntity : IEntity
    where TService : IBaseService<TEntity, TKey>
    where TMapper : IByteMapper<TDto, TEntity>
{
    protected readonly TMapper Mapper;
    protected readonly TService EntityService;
    protected readonly IStringLocalizer<TResourceSettings> Localizer;
    protected readonly IUserInformationProvider<TKey> UserInformationProvider;

    protected DtoSetController(TService entityService, TMapper mapper, IStringLocalizer<TResourceSettings> localizer, IUserInformationProvider<TKey> userInformationProvider)
    {
        Mapper = mapper;
        Localizer = localizer;
        EntityService = entityService;
        UserInformationProvider = userInformationProvider;
    }

    [HttpGet, EnableQuery]
    public virtual async Task<IQueryable<TDto>> Get(CancellationToken cancellationToken)
    {
        var query = await EntityService.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return Mapper.ToDto(query);
    }

    [HttpGet("{id:guid}")]
    public async Task<TDto> Get(TKey id, CancellationToken cancellationToken)
    {
        var entity = await EntityService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
            throw new ResourceNotFoundException(Localizer[BytePlatformStrings.General.ItemCouldNotBeFound]);

        return Mapper.ToDto(entity);
    }

    [HttpGet]
    public virtual async Task<PagedResult<TDto>> GetPage(ODataQueryOptions<TDto> options, CancellationToken cancellationToken)
    {
        IQueryable query = options.ApplyTo(await Get(cancellationToken).ConfigureAwait(false), ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.Select);
        if (query is IQueryable<TDto> resultQuery)
        {
            var totalCount = await resultQuery.LongCountAsync(cancellationToken).ConfigureAwait(false);

            resultQuery = resultQuery.SkipIf(options.Skip is not null, options.Skip!.Value)
                                     .TakeIf(options.Top is not null, options.Top!.Value);

            return new PagedResult<TDto>(await resultQuery.ToArrayAsync(cancellationToken).ConfigureAwait(false), totalCount);
        }

        throw new NotSupportedException();
    }

    [HttpPost]
    public virtual async Task<TDto> Create(TDtoAdd dto, CancellationToken cancellationToken)
    {
        var entity = Mapper.ToEntity(dto);

        await EntityService.AddAsync(entity, cancellationToken).ConfigureAwait(false);

        return Mapper.ToDto(entity);
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<TDto> Update(TKey id, TDtoEdit dto, CancellationToken cancellationToken)
    {
        if (EqualityComparer<TKey>.Default.Equals(id, dto.Id) is false)
            throw new ResourceNotFoundException(Localizer[BytePlatformStrings.General.ItemCouldNotBeFound]);

        var entity = Mapper.ToEntity(dto);

        await EntityService.EditAsync(entity, cancellationToken).ConfigureAwait(false);

        return Mapper.ToDto(entity);
    }

    [HttpDelete("{id:guid}")]
    public virtual async Task Delete(TKey id, CancellationToken cancellationToken)
    {
        await EntityService.RemoveAsync(id, cancellationToken).ConfigureAwait(false);
    }
}
