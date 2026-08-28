using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Mappings.PropertyManagement;

public static class MaintenanceRequestMappings
{
    public static MaintenanceRequestDto ToDto(MaintenanceRequest maintenanceRequest)
    {
        return new MaintenanceRequestDto
        {
            Id = maintenanceRequest.Id,
            PropertyId = maintenanceRequest.PropertyId,
            PropertyName = maintenanceRequest.Property.Name,
            Description = maintenanceRequest.Description,
            Status = maintenanceRequest.Status,
            Priority = maintenanceRequest.Priority,
            CreatedAt = maintenanceRequest.CreatedAt
        };
    }

    public static MaintenanceRequestDetailsDto ToDetailsDto(MaintenanceRequest maintenanceRequest)
    {
        return new MaintenanceRequestDetailsDto
        {
            Id = maintenanceRequest.Id,
            Property = PropertyMappings.ToDto(maintenanceRequest.Property),
            Description = maintenanceRequest.Description,
            Status = maintenanceRequest.Status,
            Priority = maintenanceRequest.Priority,
            CreatedAt = maintenanceRequest.CreatedAt
        };
    }
}
