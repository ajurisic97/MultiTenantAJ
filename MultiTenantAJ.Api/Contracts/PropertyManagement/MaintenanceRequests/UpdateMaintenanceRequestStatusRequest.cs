using MultiTenantAJ.Domain.Enums;

namespace MultiTenantAJ.Api.Contracts.PropertyManagement.MaintenanceRequests;

public record UpdateMaintenanceRequestStatusRequest(
    MaintenanceRequestStatusEnum Status);
