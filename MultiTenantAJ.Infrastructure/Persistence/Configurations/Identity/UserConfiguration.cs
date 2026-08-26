using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantAJ.Domain.Constants;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Persistence.Configurations.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users, SchemaNames.Identity);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(User.UsernameMaxLength);

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(User.PasswordHashMaxLength);

        builder.HasIndex(x => new
        {
            x.TenantId,
            x.Username
        })
        .IsUnique();

        builder.HasIndex(x => x.TenantId);
    }
}
