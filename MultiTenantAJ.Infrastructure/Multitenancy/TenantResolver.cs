using Microsoft.EntityFrameworkCore;
using MultiTenantAJ.Shared.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace MultiTenantAJ.Infrastructure.Multitenancy;

public class TenantResolver
{
    private readonly RequestDelegate _next;

    public TenantResolver(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        TenantDbContext tenantDbContext,
        CurrentTenantService currentTenantService)
    {
        if (context.Request.Headers.TryGetValue(
                MultitenancyConstants.TenantIdName,
                out var tenantHeader))
        {
            if (Guid.TryParse(tenantHeader.ToString(), out var apiKey))
            {
                var tenant = await tenantDbContext.Tenants
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.ApiKey == apiKey);

                if (tenant is null)
                {
                    context.Response.StatusCode =
                        StatusCodes.Status401Unauthorized;

                    return;
                }

                if (!tenant.IsActive)
                {
                    context.Response.StatusCode =
                        StatusCodes.Status403Forbidden;

                    return;
                }

                currentTenantService.SetTenant(tenant);
            }
        }

        await _next(context);
    }
}