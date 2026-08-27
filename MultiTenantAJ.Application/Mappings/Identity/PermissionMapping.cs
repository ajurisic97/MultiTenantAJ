using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Mappings.Identity;

public static class PermissionMappings
{
    public static PermissionDto ToDto(Permission permission)
    {
        return new PermissionDto
        {
            Id = permission.Id,
            Name = permission.Name
        };
    }
}
