namespace MultiTenantAJ.Api.Contracts.Multitenancy.Tenant;

public record UpdateTenantRequest(bool IsActive,bool MaintenanceEnabled);
