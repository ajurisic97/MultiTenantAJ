using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.Specifications;

public class RoleByIdsSpec : Specification<Role>
{
    public RoleByIdsSpec(List<Guid> roleIds)
    {
        Query.Where(x => roleIds.Contains(x.Id));
    }
}
