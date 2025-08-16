using System.Linq.Expressions;
using System.Reflection;
using BytePlatform.Server.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BytePlatform.Server.Data.Extensions;
public static class ModelBuilderExtensions
{
    public static void ConfigureDecimalPrecision(this ModelBuilder modelBuilder)
    {
        var decimalProperties = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(type => type.GetProperties())
            .Where(prop => prop.ClrType == typeof(decimal) || prop.ClrType == typeof(decimal?));

        foreach (IMutableProperty prop in decimalProperties)
        {
            prop.SetColumnType("decimal(20, 10)");
        }
    }

    public static void RegisterIsArchivedGlobalQueryFilter(this ModelBuilder modelBuilder, Assembly assembly)
    {
        Type[] archivableTypes = [.. assembly.GetExportedTypes()
                .Where(type => type.IsClass &&
                               type.IsPublic &&
                               !type.IsAbstract &&
                               typeof(IArchivableEntity).IsAssignableFrom(type))];

        foreach (Type type in archivableTypes)
        {
            // This will be same as : (entity => !entity.IsArchived)
            ParameterExpression entity = Expression.Parameter(type, type.Name.CamelCase());
            var property = type.GetProperty("IsArchived");
            if (property is null) continue;

            MemberExpression isArchivedProperty = Expression.Property(entity, property);
            LambdaExpression lambda = Expression.Lambda(Expression.Not(isArchivedProperty), entity);

            modelBuilder.Entity(type, ba => ba.HasQueryFilter(lambda));
        }
    }
}
