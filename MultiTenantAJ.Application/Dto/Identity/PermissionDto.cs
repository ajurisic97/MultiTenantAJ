using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.Identity;

public class PermissionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}
