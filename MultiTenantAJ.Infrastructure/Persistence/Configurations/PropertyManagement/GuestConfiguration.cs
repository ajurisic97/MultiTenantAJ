using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantAJ.Domain.Constants;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Persistence.Configurations.PropertyManagement;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable(TableNames.Guests, SchemaNames.PropertyManagement);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(Guest.FirstNameMaxLength);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(Guest.LastNameMaxLength);

        builder.Property(x => x.Email)
            .HasMaxLength(Guest.EmailMaxLength);

        builder.Property(x => x.Phone)
            .HasMaxLength(Guest.PhoneMaxLength);

        builder.HasIndex(x => x.TenantId);
    }
}
