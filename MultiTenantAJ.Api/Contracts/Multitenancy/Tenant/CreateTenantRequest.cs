namespace MultiTenantAJ.Api.Contracts.Multitenancy.Tenant;
public record CreateTenantRequest(
    string Id,
    string Name,
    string? ConnectionString,
    bool MaintenanceEnabled);