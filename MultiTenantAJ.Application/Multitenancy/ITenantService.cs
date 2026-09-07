using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Multitenancy;

namespace MultiTenantAJ.Application.Multitenancy;

public interface ITenantService
{
    Task<ApplicationResult<Tenant>> CreateTenantAsync(
        string id,
        string name,
        string? connectionString,
        bool maintenanceEnabled,
        CancellationToken cancellationToken = default);

    Task<ApplicationResult<Tenant>> UpdateTenantAsync(
        string id,
        bool isActive,
        bool maintenanceEnabled,
        CancellationToken cancellationToken = default);

    Task<List<Tenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default);
}


