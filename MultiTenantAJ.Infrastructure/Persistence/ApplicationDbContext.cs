using Microsoft.EntityFrameworkCore;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Catalog;
using MultiTenantAJ.Shared.Multitenancy;

namespace MultiTenantAJ.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentTenantService _currentTenantService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentTenantService currentTenantService)
        : base(options)
    {
        _currentTenantService = currentTenantService;
    }

    public DbSet<Product> Products => Set<Product>();

    public string? CurrentTenantId => _currentTenantService.TenantId;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly,
            type => type.Namespace != null &&
                    type.Namespace.Contains("Persistence.Configurations"));

        ApplyTenantQueryFilters(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        HandleTenantData();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        HandleTenantData();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTenantQueryEntityFilter<TEntity>(ModelBuilder modelBuilder)
    where TEntity : class, IMustHaveTenant
    {
        modelBuilder.Entity<TEntity>()
            .HasQueryFilter(x => x.TenantId == CurrentTenantId);
    }
    private void ApplyTenantQueryFilters(ModelBuilder modelBuilder)
    {
        #region Catalog
        ApplyTenantQueryEntityFilter<Product>(modelBuilder);
        #endregion

    }
    private void HandleTenantData()
    {
        var currentTenantId = CurrentTenantId;
        if (string.IsNullOrWhiteSpace(currentTenantId))
        {
            throw new InvalidOperationException(
                "Current tenant is not available.");
        }

        var entries = ChangeTracker
            .Entries<IMustHaveTenant>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.TenantId = currentTenantId;
            }

            if ((entry.State == EntityState.Modified ||
                entry.State == EntityState.Deleted) && entry.Entity.TenantId != currentTenantId)
            {
                throw new InvalidOperationException(
                    "Cross-tenant data modification is not allowed.");
            }
        }
    }


}