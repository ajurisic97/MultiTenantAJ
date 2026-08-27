using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Specifications;

public class UserByRefreshTokenSpec : SingleResultSpecification<User>
{
    public UserByRefreshTokenSpec(string refreshToken)
    {
        Query
            .Where(x => x.RefreshToken == refreshToken)
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions);
    }
}
