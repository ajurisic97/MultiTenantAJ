using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Authorization;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;
using MultiTenantAJ.Infrastructure.Seeder;

namespace MultiTenantAJ.Infrastructure.Multitenancy;

public class TenantService : ITenantService
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly IServiceProvider _serviceProvider;

    public TenantService(TenantDbContext tenantDbContext, IServiceProvider serviceProvider)
    {
        _tenantDbContext = tenantDbContext;
        _serviceProvider = serviceProvider;
    }

    public async Task<ApplicationResult<Tenant>> CreateTenantAsync(string id, string name, string? connectionString, bool maintenanceEnabled, CancellationToken cancellationToken = default)
    {
        var tenantExists = await _tenantDbContext.Tenants
            .AnyAsync(x => x.Id == id, cancellationToken);
        if (tenantExists)
        {
            return ApplicationResult<Tenant>.Failure(ApplicationError.Conflict("Tenant already exists."));
        }

        var tenant = Tenant.Create(
            id,
            name,
            connectionString,
            maintenanceEnabled);

        await _tenantDbContext.Tenants.AddAsync(tenant, cancellationToken);
        await _tenantDbContext.SaveChangesAsync(cancellationToken);

        await using var scope = _serviceProvider.CreateAsyncScope();
        var currentTenantService = scope.ServiceProvider.GetRequiredService<CurrentTenantService>();
        currentTenantService.SetTenant(tenant);

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await applicationDbContext.Database.MigrateAsync(cancellationToken);

        var identitySeeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
        await identitySeeder.SeedAsync(cancellationToken);

        tenant.Activate();
        await _tenantDbContext.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Tenant>.Success(tenant);
    }

    public async Task<List<Tenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default)
    {
        return await _tenantDbContext.Tenants
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<ApplicationResult<Tenant>> UpdateTenantAsync(string id, bool isActive, bool maintenanceEnabled, CancellationToken cancellationToken = default)
    {
        var tenant = await _tenantDbContext.Tenants.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (tenant == null)
        {
            return ApplicationResult<Tenant>.Failure(ApplicationError.NotFound("Tenant was not found."));
        }
        if (tenant.Id == MultitenancyConstants.RootTenantId && !isActive)
        {
            return ApplicationResult<Tenant>.Failure(
                ApplicationError.Forbidden("Root tenant cannot be deactivated."));
        }
        if (tenant.MaintenanceEnabled && !maintenanceEnabled)
        {
            await RemoveMaintenancePermissionsAsync(tenant, cancellationToken);
        }
        if (isActive)
        {
            tenant.Activate();
        }
        else
        {
            tenant.Deactivate();
        }
        tenant.Update(maintenanceEnabled);

        await _tenantDbContext.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Tenant>.Success(tenant);
    }

    private async Task RemoveMaintenancePermissionsAsync(
    Tenant tenant,
    CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var currentTenantService = scope.ServiceProvider.GetRequiredService<CurrentTenantService>();

        currentTenantService.SetTenant(tenant);

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var roleIds = await applicationDbContext.Roles
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var maintenancePermissionIds = await applicationDbContext.Permissions
            .Where(x => x.Name.StartsWith(
                $"Permissions.{ResourceCatalog.MaintenanceRequests}."))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var rolePermissions = await applicationDbContext.RolePermissions
            .Where(x =>
                roleIds.Contains(x.RoleId) &&
                maintenancePermissionIds.Contains(x.PermissionId))
            .ToListAsync(cancellationToken);

        if (rolePermissions.Count == 0)
        {
            return;
        }

        applicationDbContext.RolePermissions.RemoveRange(rolePermissions);

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
