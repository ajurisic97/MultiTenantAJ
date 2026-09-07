using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Multitenancy;

public class Tenant
{
    public string Id { get; set; } = default!;

    public Guid ApiKey { get; set; }

    public string Name { get; set; } = default!;

    public string? ConnectionString { get; set; } = default!;

    public bool IsActive { get; set; }

    public bool MaintenanceEnabled { get; set; }
    public static Tenant Create(string id, string name, string? connectionString, bool maintenanceEnabled)
    {
        return new Tenant
        {
            Id = id,
            ApiKey = Guid.NewGuid(),
            Name = name,
            ConnectionString = connectionString,
            IsActive = false,
            MaintenanceEnabled = maintenanceEnabled
        };
    }
    public void Update(bool maintenanceEnabled)
    {
        MaintenanceEnabled = maintenanceEnabled;
    }
    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
