using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Models.Identity;

public class Permission
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public ICollection<RolePermission> RolePermissions { get; set; } =  [];

    public Permission()
    {
    }

    private Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Permission Create(int id,string name)
    {
        return new Permission(id,name);
    }
}
