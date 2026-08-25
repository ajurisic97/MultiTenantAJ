using MultiTenantAJ.Shared.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Catalog;

public class Product : IMustHaveTenant
{
    public Guid Id { get; set; }

    public string TenantId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public decimal Price { get; set; }
}
