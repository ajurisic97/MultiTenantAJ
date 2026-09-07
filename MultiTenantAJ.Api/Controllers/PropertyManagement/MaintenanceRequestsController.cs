using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.PropertyManagement.MaintenanceRequests;
using MultiTenantAJ.Application.Authorization;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Create;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Delete;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.GetAll;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.GetById;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Update;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.UpdateStatus;
using MultiTenantAJ.Domain.Authorization;

namespace MultiTenantAJ.Api.Controllers.PropertyManagement;

[Tags("PropertyManagement - MaintenanceRequests")]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationPolicies.MaintenanceEnabled)]
public class MaintenanceRequestsController : ApiControllerBase
{
    private readonly ISender _sender;

    public MaintenanceRequestsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [MustHavePermission(ActionCatalog.Create, ResourceCatalog.MaintenanceRequests)]
    public async Task<IActionResult> Create(CreateMaintenanceRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateMaintenanceRequestCommand(
            request.PropertyId,
            request.Description,
            request.Priority);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpGet]
    [MustHavePermission(ActionCatalog.Search, ResourceCatalog.MaintenanceRequests)]
    public async Task<IActionResult> GetAll([FromQuery] SearchMaintenanceRequestsRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllMaintenanceRequestQuery(
            request.PropertyId,
            request.Status,
            request.Priority,
            request.FromCreationDate?.UtcDateTime,
            request.ToCreationDate?.UtcDateTime);

        var result = await _sender.Send(query, cancellationToken);

        return ResolveResult(result);
    }

    [HttpGet("{id}")]
    [MustHavePermission(ActionCatalog.View, ResourceCatalog.MaintenanceRequests)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetByIdMaintenanceRequestQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return ResolveResult(result);
    }

    [HttpPut("{id}")]
    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.MaintenanceRequests)]
    public async Task<IActionResult> Update(Guid id, UpdateMaintenanceRequestRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateMaintenanceRequestCommand(
            id,
            request.Description,
            request.Priority);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpPatch("{id}/status")]
    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.MaintenanceRequests)]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateMaintenanceRequestStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateMaintenanceRequestStatusCommand(
            id,
            request.Status);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpDelete("{id}")]
    [MustHavePermission(ActionCatalog.Delete, ResourceCatalog.MaintenanceRequests)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteMaintenanceRequestCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }
}
