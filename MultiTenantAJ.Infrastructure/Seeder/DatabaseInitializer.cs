using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Infrastructure.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;

namespace MultiTenantAJ.Infrastructure.Seeder;

public class DatabaseInitializer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly IConfiguration _configuration;
    public DatabaseInitializer(IServiceScopeFactory scopeFactory, ILogger<DatabaseInitializer> logger, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
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

        var rootTenant = tenants.Single(x => x.Id == MultitenancyConstants.RootTenantId);
        await InitializeTenantAsync(rootTenant, true, cancellationToken);

        var applicationTenants = tenants.Where(x => x.Id != MultitenancyConstants.RootTenantId).ToList();

        foreach (var tenant in applicationTenants)
        {
            try
            {
                var hasDedicatedDatabase = !string.IsNullOrWhiteSpace(tenant.ConnectionString);
                await InitializeTenantAsync(tenant, hasDedicatedDatabase, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database initialization failed for tenant {TenantId}.", tenant.Id);
            }
        }
    }

    private async Task InitializeTenantAsync(Tenant tenant, bool migrateDatabase, CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        var currentTenantService = scope.ServiceProvider.GetRequiredService<CurrentTenantService>();

        currentTenantService.SetTenant(tenant);

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (migrateDatabase)
        {
            await applicationDbContext.Database.MigrateAsync(cancellationToken);
        }
        var identitySeeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();

        await identitySeeder.SeedAsync(cancellationToken);
        var seedDemoData = _configuration.GetValue<bool>("Seeder:SeedDemoData");
        if (seedDemoData)
        {
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

            await dataSeeder.SeedAsync(cancellationToken);
        }

    }

    private async Task EnsureRootTenantAsync(TenantDbContext tenantDbContext, CancellationToken cancellationToken)
    {
        var rootTenantExists = await tenantDbContext.Tenants.AnyAsync(x => x.Id == MultitenancyConstants.RootTenantId, cancellationToken);

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