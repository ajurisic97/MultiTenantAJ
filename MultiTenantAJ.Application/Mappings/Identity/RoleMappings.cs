using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Mappings.Identity;
public static class RoleMappings
{
    public static RoleDto ToDto(Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        };
    }

    public static RoleDetailsDto ToDetailsDto(Role role)
    {
        return new RoleDetailsDto
        {
            Id = role.Id,
            Name = role.Name,
            Permissions = role.Permissions
                .Select(x => new PermissionDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList()
        };
    }
}
