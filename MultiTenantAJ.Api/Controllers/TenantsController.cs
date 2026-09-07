using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.Multitenancy.Tenant;
using MultiTenantAJ.Application.Multitenancy.Tenants.Create;
using MultiTenantAJ.Application.Multitenancy.Tenants.GetAll;
using MultiTenantAJ.Application.Multitenancy.Tenants.Update;
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
        var command = new CreateTenantCommand(request.Id, request.Name, request.ConnectionString,request.MaintenanceEnabled);
        var result = await _sender.Send(command);
        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.Tenants)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTenant(string id, UpdateTenantRequest request)
    {
        var command = new UpdateTenantCommand(id, request.IsActive, request.MaintenanceEnabled);
        var result = await _sender.Send(command);
        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.View, ResourceCatalog.Tenants)]
    [HttpGet]
    public async Task<IActionResult> GetAllTenants()
    {
        var result = await _sender.Send(new GetAllTenantsQuery());

        return ResolveResult(result);
    }


}
