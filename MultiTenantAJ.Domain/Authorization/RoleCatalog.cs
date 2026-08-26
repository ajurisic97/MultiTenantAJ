using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Authorization;
public static class RoleCatalog
{
    public const string SuperAdmin = nameof(SuperAdmin);
    public const string Admin = nameof(Admin);
    public const string User = nameof(User);
}
