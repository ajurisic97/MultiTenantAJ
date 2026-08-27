using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Permissions.Specifications;
public class PermissionByIdsSpec : Specification<Permission>
{
    public PermissionByIdsSpec(List<int> permissionIds)
    {
        Query.Where(x => permissionIds.Contains(x.Id));
    }
}
