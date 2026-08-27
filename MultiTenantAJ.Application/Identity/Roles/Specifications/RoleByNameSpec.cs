using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.Specifications;

public class RoleByNameSpec : SingleResultSpecification<Role>
{
    public RoleByNameSpec(string name)
    {
        var normalized = name.Trim().ToLower();
        Query.Where(x => x.Name.ToLower() == normalized);
    }

    public RoleByNameSpec(string name, Guid id)
    {
        //Za potrebe provjere update-> Id razlicit od trenutnog entiteta
        var normalized = name.Trim().ToLower();
        Query.Where(x => x.Id != id && x.Name.ToLower() == normalized);
    }
}
