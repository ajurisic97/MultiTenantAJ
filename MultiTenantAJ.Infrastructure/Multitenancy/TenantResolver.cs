using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using MultiTenantAJ.Domain.Multitenancy;
namespace MultiTenantAJ.Infrastructure.Multitenancy;

public class TenantResolver
{
    private readonly RequestDelegate _next;

    public TenantResolver(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, TenantDbContext tenantDbContext, CurrentTenantService currentTenantService)
    {
        Tenant? tenant;
        //prvo provjeravam ako je autentificiran korisnik da uzmem direktno iz jwt claima vrijednost tenanta. Ako nije onda iz headera gledam
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantId = context.User.FindFirst(MultitenancyConstants.TenantIdName)?.Value;
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            tenant = await tenantDbContext.Tenants.AsNoTracking().SingleOrDefaultAsync(x => x.Id == tenantId);
        }
        else
        {
            var tenantHeader = context.Request.Headers[MultitenancyConstants.TenantIdName].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(tenantHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (!Guid.TryParse(tenantHeader, out var apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            tenant = await tenantDbContext.Tenants.AsNoTracking().SingleOrDefaultAsync(x => x.ApiKey == apiKey);
        }

        if (tenant == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (!tenant.IsActive)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        currentTenantService.SetTenant(tenant);

        await _next(context);
    }
}