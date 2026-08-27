using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.Identity;

public class RoleDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<PermissionDto> Permissions { get; set; } = [];
}
