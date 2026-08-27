using Microsoft.AspNetCore.Authorization;
using MultiTenantAJ.Domain.Authorization;

namespace MultiTenantAJ.Api.Authorization;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource)
    {
        Policy = Permissions.NameFor(action, resource);
    }
}
