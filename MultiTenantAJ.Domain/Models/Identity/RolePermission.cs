using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Models.Identity;

public class RolePermission
{
    public Guid RoleId { get; set; }

    public int PermissionId { get; set; }

    public RolePermission()
    {
    }

    private RolePermission(Guid roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public static RolePermission Create(Guid roleId,int permissionId)
    {
        return new RolePermission(roleId, permissionId);
    }
}
