using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Models.Identity;

public class UserRole
{
    public Guid RoleId { get; private set; }

    public Guid UserId { get; private set; }

    public UserRole()
    {
    }

    private UserRole(Guid roleId, Guid userId)
    {
        RoleId = roleId;
        UserId = userId;
    }

    public static UserRole Assign(Guid roleId, Guid userId)
    {
        return new UserRole(roleId, userId);
    }
}
