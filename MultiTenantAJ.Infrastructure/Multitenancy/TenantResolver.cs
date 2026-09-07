using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Application.Common.Results;

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
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantId = context.User.FindFirst(MultitenancyConstants.TenantIdName)?.Value;
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                await WriteUnauthorizedAsync(context);
                return;
            }
            tenant = await tenantDbContext.Tenants
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == tenantId);
        }
        else
        {
            var tenantHeader = context.Request.Headers[MultitenancyConstants.TenantIdName].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(tenantHeader))
            {
                await WriteUnauthorizedAsync(context);
                return;
            }

            if (!Guid.TryParse(tenantHeader, out var apiKey))
            {
                await WriteUnauthorizedAsync(context);
                return;
            }
            tenant = await tenantDbContext.Tenants.AsNoTracking().SingleOrDefaultAsync(x => x.ApiKey == apiKey);
        }

        if (tenant == null)
        {
            await WriteUnauthorizedAsync(context);
            return;
        }
        if (!tenant.IsActive)
        {
            await WriteForbiddenAsync(context);
            return;
        }
        currentTenantService.SetTenant(tenant);
        await _next(context);
    }

    private static async Task WriteUnauthorizedAsync(HttpContext context)
    {
        var result = ApplicationResult<object>.Failure(
            ApplicationError.Unauthorized(
                "Tenant could not be resolved."));

        context.Response.StatusCode =
            StatusCodes.Status401Unauthorized;

        await context.Response.WriteAsJsonAsync(result);
    }

    private static async Task WriteForbiddenAsync(HttpContext context)
    {
        var result = ApplicationResult<object>.Failure(
            ApplicationError.Forbidden(
                "Tenant is inactive."));

        context.Response.StatusCode =
            StatusCodes.Status403Forbidden;

        await context.Response.WriteAsJsonAsync(result);
    }
}