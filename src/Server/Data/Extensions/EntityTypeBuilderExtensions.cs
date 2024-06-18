using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Humanizer;
using System.Linq.Expressions;
using BytePlatform.Server.Models;

namespace BytePlatform.Server.Data.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> ToTableWithPluralEntityName<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, string? name = null, bool removeSuffix = true) where TEntity : class
    {
        if (string.IsNullOrEmpty(name))
        {
            name = entityTypeBuilder.GetType().GenericTypeArguments[0].Name;
            if (removeSuffix)
            {
                name = name.Replace("Entity", null);
            }
            return entityTypeBuilder.ToTable(name.Pluralize());
        }
        else
        {
            return entityTypeBuilder.ToTable(name);
        }
    }

    public static IndexBuilder HasUniqueIndexArchivable<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object?>> indexExpression)
            where TEntity : class, IArchivableEntity
    {
        return builder
                .HasIndex(indexExpression)
                .HasFilter($"{nameof(IArchivableEntity.IsArchived)} = 0")
                .IsUnique();
    }

    public static IndexBuilder HasUniqueIndexArchivable<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object?>> indexExpression, string filter)
        where TEntity : class, IArchivableEntity
    {
        return builder
                .HasIndex(indexExpression)
                .HasFilter($"{nameof(IArchivableEntity.IsArchived)} = 0 {filter}")
                .IsUnique();
    }
}
