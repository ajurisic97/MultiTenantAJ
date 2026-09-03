using MultiTenantAJ.Domain.Enums;

namespace MultiTenantAJ.Api.Contracts.PropertyManagement.MaintenanceRequests;

public record SearchMaintenanceRequestsRequest(
    Guid? PropertyId,
    MaintenanceRequestStatusEnum? Status,
    MaintenancePriorityEnum? Priority,
    DateTimeOffset? FromCreationDate,
    DateTimeOffset? ToCreationDate);
