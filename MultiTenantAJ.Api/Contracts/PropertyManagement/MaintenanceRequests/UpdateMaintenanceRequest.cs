using MultiTenantAJ.Domain.Enums;

namespace MultiTenantAJ.Api.Contracts.PropertyManagement.MaintenanceRequests;

public record UpdateMaintenanceRequestRequest(
    string Description,
    MaintenancePriorityEnum Priority);
