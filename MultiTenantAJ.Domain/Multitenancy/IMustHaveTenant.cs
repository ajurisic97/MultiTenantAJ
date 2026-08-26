using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Multitenancy;

public interface IMustHaveTenant
{
    string TenantId { get; set; }
}
