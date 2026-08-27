using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Specifications;
public class UserByIdSpec : SingleResultSpecification<User>
{
    public UserByIdSpec(Guid id, bool includeRoles = false)
    {
        Query.Where(x => x.Id == id);

        if (includeRoles)
        {
            Query.Include(x => x.Roles);
        }
    }
}
