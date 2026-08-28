using MultiTenantAJ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.PropertyManagement;

public class MaintenanceRequestDetailsDto
{
    public Guid Id { get; set; }

    public PropertyDto Property { get; set; } = default!;

    public string Description { get; set; } = default!;

    public MaintenanceRequestStatusEnum Status { get; set; }

    public MaintenancePriorityEnum Priority { get; set; }

    public DateTime CreatedAt { get; set; }
}
