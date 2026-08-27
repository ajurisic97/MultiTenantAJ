using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.Multitenancy.Tenant;
using MultiTenantAJ.Domain.Authorization;
using MultiTenantAJ.Infrastructure.Multitenancy;

namespace MultiTenantAJ.Api.Controllers;

[Route("api/[controller]")]
public class TenantsController : ApiControllerBase
{
    private readonly TenantService _tenantService;

    public TenantsController(TenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [MustHavePermission(ActionCatalog.Create, ResourceCatalog.Tenants)]
    [HttpPost]
    public async Task<IActionResult> CreateTenant(CreateTenantRequest request)
    {
        var result = await _tenantService.CreateTenantAsync(request.Id, request.Name, request.ConnectionString);
        return ResolveResult(result);
    }
}
