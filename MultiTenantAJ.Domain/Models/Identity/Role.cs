using MultiTenantAJ.Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace MultiTenantAJ.Domain.Models.Identity;

public class Role : IMustHaveTenant
{
    public Guid Id { get; private set; }
    public string TenantId { get; set; } = default!;

    public string Name { get; private set; } = default!;

    public string? Description { get; private set; }

    public ICollection<Permission> Permissions { get; set; } =  [];

    public ICollection<User> Users { get; set; } =  [];

    public Role()
    {
    }

    private Role(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static Role Create(string name, string? description)
    {
        return new Role(Guid.NewGuid(), name, description);
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
