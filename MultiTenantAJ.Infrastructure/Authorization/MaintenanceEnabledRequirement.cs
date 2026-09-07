using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Authorization;

public class MaintenanceEnabledRequirement : IAuthorizationRequirement
{
}
