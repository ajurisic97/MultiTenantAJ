using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.Identity;
public class UserDetailsDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public List<RoleDto> Roles { get; set; } = [];
}
