using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantAJ.Domain.Constants;
using MultiTenantAJ.Domain.Models.PropertyManagement;

namespace MultiTenantAJ.Infrastructure.Persistence.Configurations.PropertyManagement;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable(TableNames.Properties, SchemaNames.PropertyManagement);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Property.NameMaxLength);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(Property.AddressMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(Property.DescriptionMaxLength);

        builder.HasIndex(x => x.TenantId);
    }
}
