﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BytePlatform.Server.Data.Configurations;
public partial class SqlServerPrimaryKeySequentialGuidDefaultValueConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (var prop in entityType.GetKeys().SelectMany(k => k.Properties).Where(p => p.ClrType == typeof(Guid)))
            {
                prop.SetDefaultValueSql("NewSequentialID()");
            }
        }
    }
}