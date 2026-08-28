using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.PropertyManagement.Properties;
using MultiTenantAJ.Application.PropertyManagement.Properties.Create;
using MultiTenantAJ.Application.PropertyManagement.Properties.Delete;
using MultiTenantAJ.Application.PropertyManagement.Properties.GetAll;
using MultiTenantAJ.Application.PropertyManagement.Properties.GetById;
using MultiTenantAJ.Application.PropertyManagement.Properties.Update;
using MultiTenantAJ.Domain.Authorization;

namespace MultiTenantAJ.Api.Controllers.PropertyManagement;

[Tags("PropertyManagement - Properties")]
[Route("api/[controller]")]
public class PropertiesController : ApiControllerBase
{
    private readonly ISender _sender;

    public PropertiesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [MustHavePermission(ActionCatalog.Create, ResourceCatalog.Properties)]
    public async Task<IActionResult> Create(CreatePropertyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreatePropertyCommand(
            request.Name,
            request.Address,
            request.Description,
            request.NumberOfRooms,
            request.NumberOfBeds,
            request.MaximumCapacity);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpGet]
    [MustHavePermission(ActionCatalog.Search, ResourceCatalog.Properties)]
    public async Task<IActionResult> GetAll([FromQuery] SearchPropertyRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllPropertyQuery(
            request.Name,
            request.Address,
            request.IsActive,
            request.MinimumCapacity);

        var result = await _sender.Send(query, cancellationToken);

        return ResolveResult(result);
    }

    [HttpGet("{id}")]
    [MustHavePermission(ActionCatalog.View, ResourceCatalog.Properties)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetByIdPropertyQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return ResolveResult(result);
    }

    [HttpPut("{id}")]
    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.Properties)]
    public async Task<IActionResult> Update(Guid id, UpdatePropertyRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdatePropertyCommand(
            id,
            request.Name,
            request.Address,
            request.Description,
            request.NumberOfRooms,
            request.NumberOfBeds,
            request.MaximumCapacity,
            request.IsActive);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpDelete("{id}")]
    [MustHavePermission(ActionCatalog.Delete, ResourceCatalog.Properties)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeletePropertyCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }
}
