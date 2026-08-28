using MultiTenantAJ.Domain.Enums;

namespace MultiTenantAJ.Api.Contracts.PropertyManagement.MaintenanceRequests;
public record CreateMaintenanceRequest(
    Guid PropertyId,
    string Description,
    MaintenancePriorityEnum Priority);