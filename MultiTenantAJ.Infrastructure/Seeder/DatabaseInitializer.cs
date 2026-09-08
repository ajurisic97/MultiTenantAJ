using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Infrastructure.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;
using Npgsql;

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
        var seedDemoData = _configuration.GetValue<bool>("Seeder:SeedDemoData");

        if (seedDemoData)
        {
            await EnsureDemoTenantsAsync(tenantDbContext, cancellationToken);
        }

        var tenants = await tenantDbContext.Tenants
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var rootTenant = tenants.Single(x => x.Id == MultitenancyConstants.RootTenantId);
        await InitializeTenantAsync(rootTenant, true, false, cancellationToken);

        var applicationTenants = tenants.Where(x => x.Id != MultitenancyConstants.RootTenantId).ToList();

        foreach (var tenant in applicationTenants)
        {
            try
            {
                var hasSeparatedDatabase = !string.IsNullOrWhiteSpace(tenant.ConnectionString);
                await InitializeTenantAsync(tenant, hasSeparatedDatabase, seedDemoData, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database initialization failed for tenant {TenantId}.", tenant.Id);
            }
        }
    }

    private async Task InitializeTenantAsync(Tenant tenant, bool migrateDatabase, bool seedDemoData, CancellationToken cancellationToken)
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

        if (seedDemoData && IsDemoTenant(tenant.Id))
        {
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            await dataSeeder.SeedAsync(cancellationToken);
        }

    }

    private static bool IsDemoTenant(string tenantId)
    {
        List<string> demoTenantIds =
        [
            MultitenancyConstants.AdriaStayTenantId,
            MultitenancyConstants.DalmatiaRentalsTenantId,
            MultitenancyConstants.SibenikTravelTenantId,
            MultitenancyConstants.JadranApartmentsTenantId
        ];
        return demoTenantIds.Contains(tenantId);
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
            IsActive = true,
            MaintenanceEnabled = true,
        };

        await tenantDbContext.Tenants.AddAsync(rootTenant, cancellationToken);
        await tenantDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureDemoTenantsAsync(TenantDbContext tenantDbContext, CancellationToken cancellationToken)
    {
        var defaultConnectionString = _configuration.GetConnectionString("Database") ?? throw new InvalidOperationException("Connection string 'Database' is not configured.");

        var secondaryConnectionStringBuilder = new NpgsqlConnectionStringBuilder(defaultConnectionString)
        {
            Database = "SecondaryTenantDb"
        };

        var secondaryTenantConnectionString = secondaryConnectionStringBuilder.ConnectionString;
        var demoTenants = new List<Tenant>
        {
            new Tenant
            {
                Id = MultitenancyConstants.AdriaStayTenantId,
                ApiKey = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Adria Stay",
                ConnectionString = null,
                IsActive = true,
                MaintenanceEnabled = true
            },
            new Tenant
            {
                Id = MultitenancyConstants.DalmatiaRentalsTenantId,
                ApiKey = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Dalmatia Rentals",
                ConnectionString = null,
                IsActive = true,
                MaintenanceEnabled = true
            },
            new Tenant
            {
                Id = MultitenancyConstants.SibenikTravelTenantId,
                ApiKey = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Šibenik Travel",
                ConnectionString = secondaryTenantConnectionString, 
                IsActive = true,
                MaintenanceEnabled = true
            },
            new Tenant
            {
                Id = MultitenancyConstants.JadranApartmentsTenantId,
                ApiKey = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Jadran Apartments",
                ConnectionString = secondaryTenantConnectionString, 
                IsActive = true,
                MaintenanceEnabled = false
            }
        };

        var existingTenantIds = await tenantDbContext.Tenants.Select(x => x.Id).ToListAsync(cancellationToken);
        var missingTenants = demoTenants.Where(x => !existingTenantIds.Contains(x.Id)).ToList();

        if (missingTenants.Count == 0)
        {
            return;
        }

        await tenantDbContext.Tenants.AddRangeAsync(missingTenants,cancellationToken);
        await tenantDbContext.SaveChangesAsync(cancellationToken);
    }
}