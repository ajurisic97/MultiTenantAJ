using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.Specifications;

public class RoleByIdSpec : SingleResultSpecification<Role>
{
    public RoleByIdSpec(Guid id, bool includePermissions = false, bool includeUsers = false)
    {
        Query
            .Where(x => x.Id == id);

        if (includePermissions)
        {
            Query.Include(x => x.Permissions);
        }
        if (includeUsers)
        {
            Query.Include(x => x.Users);
        }
    }
}
