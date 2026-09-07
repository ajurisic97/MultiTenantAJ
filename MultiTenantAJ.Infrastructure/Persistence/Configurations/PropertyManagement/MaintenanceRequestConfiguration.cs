using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantAJ.Domain.Constants;
using MultiTenantAJ.Domain.Models.PropertyManagement;

namespace MultiTenantAJ.Infrastructure.Persistence.Configurations.PropertyManagement;

public class MaintenanceRequestConfiguration : IEntityTypeConfiguration<MaintenanceRequest>
{
    public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
    {
        builder.ToTable(TableNames.MaintenanceRequests, SchemaNames.PropertyManagement);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(MaintenanceRequest.DescriptionMaxLength);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Priority)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(x => x.Property)
            .WithMany(x => x.MaintenanceRequests)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.TenantId);
    }
}
