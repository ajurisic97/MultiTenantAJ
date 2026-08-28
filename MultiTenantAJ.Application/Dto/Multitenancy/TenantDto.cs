using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.Multitenancy;

public class TenantDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public Guid ApiKey { get; set; }
    public bool IsActive { get; set; }
}
