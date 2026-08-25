using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Multitenancy;

public class CurrentTenantService : ICurrentTenantService
{
    public string? TenantId { get; private set; }

    public string? ConnectionString { get; private set; }

    public bool IsActive { get; private set; }

    internal void SetTenant(Tenant tenant)
    {
        TenantId = tenant.Id;
        ConnectionString = tenant.ConnectionString;
        IsActive = tenant.IsActive;
    }
}
