using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.Identity;
public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
}
