using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantAJ.Domain.Constants;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Persistence.Configurations.PropertyManagement;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable(TableNames.Reservations, SchemaNames.PropertyManagement);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(x => x.Property)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Guest)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.TenantId);
    }
}
