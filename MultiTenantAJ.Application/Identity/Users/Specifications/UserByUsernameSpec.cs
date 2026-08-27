using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Specifications;

public class UserByUsernameSpec : SingleResultSpecification<User>
{
    public UserByUsernameSpec(string username, bool includeRoles = true)
    {
        var normalized = username.Trim().ToLower();
        Query
            .Where(x => x.Username.ToLower() == normalized);
        if (includeRoles)
        {
            Query
                .Include(x => x.Roles)
                .ThenInclude(x => x.Permissions);
        }
            
    }

    public UserByUsernameSpec(string username, Guid excludedId)
    {
        var normalized = username.Trim().ToLower();

        Query.Where(x =>
            x.Id != excludedId &&
            x.Username.ToLower() == normalized);
    }
}
