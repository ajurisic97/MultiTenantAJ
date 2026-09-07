using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantAJ.Domain.Constants;
using MultiTenantAJ.Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Multitenancy.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable(TableNames.Tenants, SchemaNames.MultiTenancy);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ApiKey)
            .IsRequired();

        builder.HasIndex(x => x.ApiKey)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ConnectionString);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.MaintenanceEnabled)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
