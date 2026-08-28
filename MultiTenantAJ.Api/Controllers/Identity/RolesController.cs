using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.Identity.Roles;
using MultiTenantAJ.Application.Identity.Roles.Create;
using MultiTenantAJ.Application.Identity.Roles.Delete;
using MultiTenantAJ.Application.Identity.Roles.GetAll;
using MultiTenantAJ.Application.Identity.Roles.GetById;
using MultiTenantAJ.Application.Identity.Roles.Update;
using MultiTenantAJ.Application.Identity.Roles.UpdateRolePermissions;
using MultiTenantAJ.Domain.Authorization;

namespace MultiTenantAJ.Api.Controllers.Identity;

[Route("api/[controller]")]
public class RolesController : ApiControllerBase
{
    private readonly ISender _sender;

    public RolesController(ISender sender)
    {
        _sender = sender;
    }

    [MustHavePermission(ActionCatalog.Search, ResourceCatalog.Roles)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllRoleQuery();
        var result = await _sender.Send(query);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.View, ResourceCatalog.Roles)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdRoleQuery(id);
        var result = await _sender.Send(query);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Create, ResourceCatalog.Roles)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleRequest request)
    {
        var command = new CreateRoleCommand(request.Name, request.Description);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.Roles)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateRoleRequest request)
    {
        var command = new UpdateRoleCommand(
            id,
            request.Name,
            request.Description);

        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Delete, ResourceCatalog.Roles)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteRoleCommand(id);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.Roles)]
    [HttpPut("{id}/permissions")]
    public async Task<IActionResult> UpdatePermissions(Guid id, UpdateRolePermissionsRequest request)
    {
        var command = new UpdateRolePermissionsCommand(id, request.PermissionIds);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }
}
