using Microsoft.AspNetCore.Authorization;
using MultiTenantAJ.Application.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Authorization;

public class MaintenanceEnabledAuthorizationHandler: AuthorizationHandler<MaintenanceEnabledRequirement>
{
    private readonly ICurrentTenantService _currentTenantService;

    public MaintenanceEnabledAuthorizationHandler(ICurrentTenantService currentTenantService)
    {
        _currentTenantService = currentTenantService;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MaintenanceEnabledRequirement requirement)
    {
        if (_currentTenantService.MaintenanceEnabled)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
