using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Authorization;

public sealed record PermissionDefinition(
    string Name,
    bool IsRootOnly = false);
