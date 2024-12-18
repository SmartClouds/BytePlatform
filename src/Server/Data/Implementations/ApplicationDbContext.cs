using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using BytePlatform.Server.Data.Configurations;
using BytePlatform.Server.Data.Contracts;
using BytePlatform.Server.Extensions;
using BytePlatform.Server.Models;
using BytePlatform.Server.Models.Identity;
using BytePlatform.Shared.Exceptions;
using BytePlatform.Shared.Resources;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BytePlatform.Server.Data.Implementations;

public abstract class ApplicationDbContext<TKey, TUser, TRole> : IdentityDbContext<TUser, TRole, TKey, UserClaimEntity<TKey>, UserRoleEntity<TKey>, UserLoginEntity<TKey>, RoleClaimEntity<TKey>, UserTokenEntity<TKey>>, IDataProtectionKeyContext, IDbContext
    where TKey : IEquatable<TKey>
    where TUser : UserEntity<TKey>
    where TRole : RoleEntity<TKey>
{
    protected abstract string AssemblyName { get; }
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        RegisterDbSets(builder);

        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        ConfigIdentityTables(builder);

        AddSequentialGuidForIds(builder);

        ConfigureDecimalPrecision(builder);

        ConcurrencyStamp(builder);

        ConfigureCascades(builder);

        RegisterIsArchivedGlobalQueryFilter(builder);

        SeedDatabase(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new SqlServerPrimaryKeySequentialGuidDefaultValueConvention());

        base.ConfigureConventions(configurationBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        try
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConflictException(BytePlatformStrings.General.UpdateConcurrencyException, exception);
        }
        // https://docs.microsoft.com/en-us/sql/relational-databases/errors-events/database-engine-events-and-errors?view=sql-server-ver15
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && sqlException.Class == 14 && sqlException.Number == 2601)
        {
            var innerExceptionMessage = ex.InnerException.Message;

            string approximateEntityName = GetApproximateEntityName(innerExceptionMessage);

            var involvedColumnNames = GetInvolvedColumnNames(innerExceptionMessage);

            string[] duplicateKeyValueItems = GetDuplicateKeyValueItems(innerExceptionMessage);

            var entity = ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged)
                .First(e => e.Metadata.DisplayName().StartsWith(approximateEntityName))
                .Entity;

            var possiblePropNames = GetPossiblePropertyNames(entity, duplicateKeyValueItems);

            var finalPropNames = possiblePropNames
                                                .Where(p => involvedColumnNames.Contains(p)) // To ensure that there is no contradiction in the result
                                                .Where(p => Guid.TryParse(p, out var _) is false) // To remove ids (Guid)
                                                .ToArray();

            // from here onwards needs to be discussed
            if (finalPropNames.Length > 0)
            {
                throw new ResourceValidationException(BytePlatformStrings.General.PropertyAlreadyExists, new ErrorResourcePayload
                {
                    ResourceTypeName = entity.GetType().FullName,
                    Details = finalPropNames.Select(p => new PropertyErrorResourceCollection
                    {
                        Name = p,
                        Errors =
                        [
                            new() {
                                Key = BytePlatformStrings.General.PropertyAlreadyExists,
                                Message = null
                            }
                        ]
                    }).ToList()
                });
            }

            throw;
        }
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConflictException(BytePlatformStrings.General.UpdateConcurrencyException, exception);
        }
        // https://docs.microsoft.com/en-us/sql/relational-databases/errors-events/database-engine-events-and-errors?view=sql-server-ver15
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && sqlException.Class == 14 && sqlException.Number == 2601)
        {
            var innerExceptionMessage = ex.InnerException.Message;

            string approximateEntityName = GetApproximateEntityName(innerExceptionMessage);

            var involvedColumnNames = GetInvolvedColumnNames(innerExceptionMessage);

            string[] duplicateKeyValueItems = GetDuplicateKeyValueItems(innerExceptionMessage);

            var entity = ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged)
                .First(e => e.Metadata.DisplayName().StartsWith(approximateEntityName))
                .Entity;

            var possiblePropNames = GetPossiblePropertyNames(entity, duplicateKeyValueItems);

            var finalPropNames = possiblePropNames
                                                .Where(p => involvedColumnNames.Contains(p)) // To ensure that there is no contradiction in the result
                                                .Where(p => Guid.TryParse(p, out var _) is false) // To remove ids (Guid)
                                                .ToArray();

            // from here onwards needs to be discussed
            if (finalPropNames.Length > 0)
            {
                throw new ResourceValidationException(BytePlatformStrings.General.PropertyAlreadyExists, new ErrorResourcePayload
                {
                    ResourceTypeName = entity.GetType().FullName,
                    Details = finalPropNames.Select(p => new PropertyErrorResourceCollection
                    {
                        Name = p,
                        Errors =
                        [
                            new() {
                                Key = BytePlatformStrings.General.PropertyAlreadyExists,
                                Message = null
                            }
                        ]
                    }).ToList()
                });
            }

            throw;
        }
    }

    #region Utilities

    protected virtual void RegisterDbSets(ModelBuilder builder)
    {
        var identityTypes = new[] {
            typeof(UserEntity<TKey>),
            typeof(RoleEntity<TKey>),
            typeof(UserRoleEntity<TKey>),
            typeof(RoleClaimEntity<TKey>),
            typeof(UserLoginEntity<TKey>),
            typeof(UserTokenEntity<TKey>),
            typeof(UserClaimEntity<TKey>)
        };

        IEnumerable<Type> entityTypes = typeof(TUser)
            .Assembly
            .GetExportedTypes()
            .Where(type => type.IsClass &&
                           !type.IsAbstract &&
                           typeof(IEntity).IsAssignableFrom(type) &&
                           !identityTypes.Any(identityType => identityType.IsAssignableFrom(type))) //Remove identity table from list of DbSets
            .ToArray();

        foreach (var entity in entityTypes)
        {
            builder.Entity(entity);
        }
    }

    protected virtual void ConfigIdentityTables(ModelBuilder builder)
    {
        //Config Asp Identity table name
        builder.Entity<TUser>().ToTable("Users");
        builder.Entity<TRole>().ToTable("Roles");
        builder.Entity<UserRoleEntity<TKey>>().ToTable("UserRoles");
        builder.Entity<RoleClaimEntity<TKey>>().ToTable("RoleClaims");
        builder.Entity<UserLoginEntity<TKey>>().ToTable("UserLogins");
        builder.Entity<UserTokenEntity<TKey>>().ToTable("UserTokens");
        builder.Entity<UserClaimEntity<TKey>>().ToTable("UserClaims");

        builder.Entity<UserClaimEntity<TKey>>(b =>
        {
            // Primary key
            b.HasKey(uc => uc.Id);
            b.Property(uc => uc.Id).HasColumnType("uniqueidentifier");

            // Maps to the AspNetUserClaims table
            b.ToTable("UserClaims");
        });

        builder.Entity<RoleClaimEntity<TKey>>(b =>
        {
            // Primary key
            b.HasKey(rc => rc.Id);
            b.Property(rc => rc.Id).HasColumnType("uniqueidentifier");

            // Maps to the AspNetRoleClaims table
            b.ToTable("RoleClaims");
        });
    }

    protected virtual void RegisterIsArchivedGlobalQueryFilter(ModelBuilder builder)
    {
        Type[] archivableTypes = Assembly.Load(AssemblyName).GetExportedTypes()
                .Where(type => type.IsClass &&
                               type.IsPublic &&
                               !type.IsAbstract &&
                               typeof(IArchivableEntity).IsAssignableFrom(type))
                .ToArray();

        foreach (Type type in archivableTypes)
        {
            // This will be same as : (entity => !entity.IsArchived)
            ParameterExpression entity = Expression.Parameter(type, type.Name.CamelCase());
            var property = type.GetProperty("IsArchived");
            if (property is null) continue;

            MemberExpression isArchivedProperty = Expression.Property(entity, property);
            LambdaExpression lambda = Expression.Lambda(Expression.Not(isArchivedProperty), entity);

            builder.Entity(type, ba => ba.HasQueryFilter(lambda));
        }
    }

    protected virtual void AddSequentialGuidForIds(ModelBuilder builder)
    {
        foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
        {
            IMutableProperty? property = entityType.GetProperties().SingleOrDefault(p => p.Name.Equals(nameof(IEntity<Guid>.Id), StringComparison.OrdinalIgnoreCase));
            if (property is not null && property.ClrType == typeof(Guid))
            {
                var p = builder.Entity(entityType.ClrType).Property(property.Name);

                p.ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NewSequentialID()");
            }
        }
    }

    protected virtual void ConfigureDecimalPrecision(ModelBuilder builder)
    {
        var decimalProperties = builder.Model
            .GetEntityTypes()
            .SelectMany(type => type.GetProperties())
            .Where(prop => prop.ClrType == typeof(decimal) || prop.ClrType == typeof(decimal?));

        foreach (IMutableProperty prop in decimalProperties)
        {
            prop.SetColumnType("decimal(20, 10)");
        }
    }

    protected virtual void ConfigureCascades(ModelBuilder builder)
    {
        IEnumerable<IMutableForeignKey> cascadeFKs = builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (IMutableForeignKey fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;
    }

    protected virtual void SeedDatabase(ModelBuilder builder)
    {
    }


    private static List<string> GetPossiblePropertyNames(object entity, string[] duplicateKeyValueItems)
    {
        var possiblePropNames = new List<string>();
        foreach (var property in entity.GetType().GetProperties())
        {
            var val = property.GetValue(entity);
            if (duplicateKeyValueItems.Contains(val?.ToString()))
            {
                possiblePropNames.Add(property.Name);
            }
        }
        return possiblePropNames;
    }

    private static string[] GetDuplicateKeyValueItems(string input)
    {
        var duplicateKeyValueRegex = new Regex(@"\(.+\)");

        //Duplicate Key Values
        return duplicateKeyValueRegex
                .Match(input).Value
                .Trim('(', ')')
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static string[] GetInvolvedColumnNames(string input)
    {
        var indexNameRegex = new Regex(@"'IX_.+.+'");
        var extractedIndexName = indexNameRegex.Match(input).Value;
        var removeContentionFromTheEnd = extractedIndexName.Remove(extractedIndexName.Length - 1, 1);

        //Involved Column Names
        return removeContentionFromTheEnd.Split('_', StringSplitOptions.RemoveEmptyEntries).Skip(2).ToArray();
    }

    private static string GetApproximateEntityName(string input)
    {
        var tableNameRegex = new Regex(@"(?<=dbo.)(.*?)(?=')");
        string extractedEntityName = tableNameRegex.Match(input).Value;

        int removeLength = extractedEntityName.Length > 6 ? extractedEntityName.Length - 5 : extractedEntityName.Length - 3;

        //Approximate Entity Name
        return extractedEntityName.Remove(removeLength);
    }

    private void ConcurrencyStamp(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties()
                .Where(p => p.Name is "ConcurrencyStamp" && p.PropertyInfo?.PropertyType == typeof(byte[])))
            {
                var builder = new PropertyBuilder(property);
                builder.IsConcurrencyToken()
                    .IsRowVersion();

            }
        }
    }

    #endregion Utilities

    #region Methods

    /// <summary>
    /// Creates a DbSet that can be used to query and save instances of entity
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>A set for the given entity type</returns>
    public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        return base.Set<TEntity>();
    }

    /// <summary>
    /// Generate a script to create all tables for the current model
    /// </summary>
    /// <returns>A SQL script</returns>
    public virtual string GenerateCreateScript()
    {
        return Database.GenerateCreateScript();
    }

    /// <summary>
    /// Executes the given SQL against the database
    /// </summary>
    /// <param name="sql">The SQL to execute</param>
    /// <param name="doNotEnsureTransaction">true - the transaction creation is not ensured; false - the transaction creation is ensured.</param>
    /// <param name="timeout">The timeout to use for command. Note that the command timeout is distinct from the connection timeout, which is commonly set on the database connection string</param>
    /// <param name="parameters">Parameters to use with the SQL</param>
    /// <returns>The number of rows affected</returns>
    public virtual async Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, bool doNotEnsureTransaction, int? timeout, CancellationToken cancellationToken)
    {
        //set specific command timeout
        var previousTimeout = Database.GetCommandTimeout();
        Database.SetCommandTimeout(timeout);

        var result = 0;
        if (doNotEnsureTransaction is false)
        {
            //use with transaction
            using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            result = await Database.ExecuteSqlRawAsync(sql, parameters);
            await transaction.CommitAsync(cancellationToken);
        }
        else
            result = await Database.ExecuteSqlRawAsync(sql, parameters);

        //return previous timeout back
        Database.SetCommandTimeout(previousTimeout);

        return result;
    }

    /// <summary>
    /// Detach an entity from the context
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="entity">Entity</param>
    public virtual void Detach<TEntity>(TEntity entity) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entity);

        var entityEntry = Entry(entity);
        if (entityEntry is null)
            return;

        //set the entity is not being tracked by the context
        entityEntry.State = EntityState.Detached;
    }

    #endregion
}
