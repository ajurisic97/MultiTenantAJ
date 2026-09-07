using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.Multitenancy.Tenant;
using MultiTenantAJ.Application.Multitenancy.Tenants.Create;
using MultiTenantAJ.Domain.Authorization;
using MultiTenantAJ.Infrastructure.Multitenancy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MultiTenantAJ.Api.Controllers;

[Route("api/[controller]")]
public class TenantsController : ApiControllerBase
{
    private readonly ISender _sender;

    public TenantsController(ISender sender)
    {
        _sender = sender;
    }

    [MustHavePermission(ActionCatalog.Create, ResourceCatalog.Tenants)]
    [HttpPost]
    public async Task<IActionResult> CreateTenant(CreateTenantRequest request)
    {
        var command = new CreateTenantCommand(request.Id, request.Name, request.ConnectionString);
        var result = await _sender.Send(command);
        return ResolveResult(result);
    }
}
