using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Contracts.Multitenancy.Tenant;
using MultiTenantAJ.Infrastructure.Multitenancy;

namespace MultiTenantAJ.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TenantsController : ControllerBase
{
    private readonly TenantService _tenantService;

    public TenantsController(TenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTenant(
        CreateTenantRequest request,
        CancellationToken cancellationToken)
    {
        var tenant = await _tenantService.CreateTenantAsync(
            request.Id,
            request.Name,
            request.ConnectionString,
            cancellationToken);

        return Ok(new
        {
            tenant.Id,
            tenant.Name,
            tenant.ApiKey,
            tenant.IsActive
        });
    }
}
