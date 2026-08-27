using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Specifications;

public class UserByUsernameSpec : Specification<User>
{
    public UserByUsernameSpec(string username)
    {
        Query
            .Where(x => x.Username == username)
            .Include(x => x.Roles)
                .ThenInclude(x => x.Permissions);
    }
}
