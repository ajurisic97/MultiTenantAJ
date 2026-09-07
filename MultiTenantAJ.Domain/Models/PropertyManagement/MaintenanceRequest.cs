using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Models.PropertyManagement;

public class MaintenanceRequest : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public string TenantId { get; set; } = default!;
    public Guid PropertyId { get; private set; }
    public string Description { get; private set; } = default!;
    public MaintenanceRequestStatusEnum Status { get; private set; }
    public MaintenancePriorityEnum Priority { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Property Property { get; private set; } = default!;

    #region const

    public const int DescriptionMaxLength = 1000;

    #endregion

    public MaintenanceRequest()
    {
    }

    private MaintenanceRequest(Guid propertyId, string description, MaintenancePriorityEnum priority)
    {
        Id = Guid.NewGuid();
        PropertyId = propertyId;
        Description = description;
        Priority = priority;
        Status = MaintenanceRequestStatusEnum.Open;
        CreatedAt = DateTime.UtcNow;
    }

    public static MaintenanceRequest Create(Guid propertyId, string description, MaintenancePriorityEnum priority)
    {
        return new MaintenanceRequest(propertyId, description, priority);
    }

    public void Update(string description, MaintenancePriorityEnum priority)
    {
        Description = description;
        Priority = priority;
    }

    public void StartProgress()
    {
        Status = MaintenanceRequestStatusEnum.InProgress;
    }

    public void Resolve()
    {
        Status = MaintenanceRequestStatusEnum.Resolved;
    }

    public void Close()
    {
        Status = MaintenanceRequestStatusEnum.Closed;
    }

    public void Cancel()
    {
        Status = MaintenanceRequestStatusEnum.Cancelled;
    }
}
