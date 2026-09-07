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
}
