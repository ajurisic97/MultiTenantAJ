using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.PropertyManagement.Guests;
using MultiTenantAJ.Application.PropertyManagement.Guests.Create;
using MultiTenantAJ.Application.PropertyManagement.Guests.Delete;
using MultiTenantAJ.Application.PropertyManagement.Guests.GetAll;
using MultiTenantAJ.Application.PropertyManagement.Guests.GetById;
using MultiTenantAJ.Application.PropertyManagement.Guests.Update;
using MultiTenantAJ.Domain.Authorization;

namespace MultiTenantAJ.Api.Controllers.PropertyManagement;

[Tags("PropertyManagement - Guests")]
[Route("api/[controller]")]
public class GuestsController : ApiControllerBase
{
    private readonly ISender _sender;

    public GuestsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [MustHavePermission(ActionCatalog.Create, ResourceCatalog.Guests)]
    public async Task<IActionResult> Create(CreateGuestRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateGuestCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpGet]
    [MustHavePermission(ActionCatalog.Search, ResourceCatalog.Guests)]
    public async Task<IActionResult> GetAll([FromQuery] SearchGuestRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllGuestQuery(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone);

        var result = await _sender.Send(query, cancellationToken);

        return ResolveResult(result);
    }

    [HttpGet("{id}")]
    [MustHavePermission(ActionCatalog.View, ResourceCatalog.Guests)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetByIdGuestQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return ResolveResult(result);
    }

    [HttpPut("{id}")]
    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.Guests)]
    public async Task<IActionResult> Update(Guid id, UpdateGuestRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateGuestCommand(
            id,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpDelete("{id}")]
    [MustHavePermission(ActionCatalog.Delete, ResourceCatalog.Guests)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteGuestCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }
}
