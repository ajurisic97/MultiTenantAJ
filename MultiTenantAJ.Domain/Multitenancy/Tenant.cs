using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Multitenancy;

public class Tenant
{
    public string Id { get; set; } = default!;
    public string Identifier { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
    public bool IsActive { get; set; }
}
