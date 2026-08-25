using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Multitenancy;

public interface ICurrentTenantService
{
    string? TenantId { get; }

    string? ConnectionString { get; }

    bool IsActive { get; }
}
