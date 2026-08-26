using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Infrastructure.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;

namespace MultiTenantAJ.Infrastructure.Seeder;

public class DatabaseInitializer
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DatabaseInitializer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var tenantScope = _scopeFactory.CreateAsyncScope();

        var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<TenantDbContext>();

        await tenantDbContext.Database.MigrateAsync(cancellationToken);

        await EnsureRootTenantAsync(tenantDbContext, cancellationToken);

        var tenants = await tenantDbContext.Tenants
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var tenant in tenants)
        {
            await InitializeTenantAsync(tenant, cancellationToken);
        }
    }

    private async Task InitializeTenantAsync(Tenant tenant, CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        var currentTenantService = scope.ServiceProvider.GetRequiredService<CurrentTenantService>();
        currentTenantService.SetTenant(tenant);

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await applicationDbContext.Database.MigrateAsync(cancellationToken);

        var identitySeeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();

        await identitySeeder.SeedAsync(cancellationToken);
    }

    private async Task EnsureRootTenantAsync(TenantDbContext tenantDbContext, CancellationToken cancellationToken)
    {
        var rootTenantExists = await tenantDbContext.Tenants
            .AnyAsync(x => x.Id == MultitenancyConstants.RootTenantId, cancellationToken);

        if (rootTenantExists)
        {
            return;
        }

        var rootTenant = new Tenant
        {
            Id = MultitenancyConstants.RootTenantId,
            ApiKey = Guid.NewGuid(),
            Name = "Root",
            ConnectionString = null,
            IsActive = true
        };

        await tenantDbContext.Tenants.AddAsync(rootTenant, cancellationToken);
        await tenantDbContext.SaveChangesAsync(cancellationToken);
    }
}