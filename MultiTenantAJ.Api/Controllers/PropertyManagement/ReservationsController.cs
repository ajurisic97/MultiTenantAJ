using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.PropertyManagement.Reservations;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Create;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Delete;
using MultiTenantAJ.Application.PropertyManagement.Reservations.GetAll;
using MultiTenantAJ.Application.PropertyManagement.Reservations.GetById;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Update;
using MultiTenantAJ.Application.PropertyManagement.Reservations.UpdateStatus;
using MultiTenantAJ.Domain.Authorization;

namespace MultiTenantAJ.Api.Controllers.PropertyManagement;

[Tags("PropertyManagement - Reservations")]
[Route("api/[controller]")]
public class ReservationsController : ApiControllerBase
{
    private readonly ISender _sender;

    public ReservationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [MustHavePermission(ActionCatalog.Create, ResourceCatalog.Reservations)]
    public async Task<IActionResult> Create(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateReservationCommand(
            request.PropertyId,
            request.GuestId,
            request.StartDate,
            request.EndDate,
            request.NumberOfGuests);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpGet]
    [MustHavePermission(ActionCatalog.Search, ResourceCatalog.Reservations)]
    public async Task<IActionResult> GetAll([FromQuery] SearchReservationRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllReservationQuery(
            request.PropertyId,
            request.GuestId,
            request.Status,
            request.FromDate,
            request.ToDate);

        var result = await _sender.Send(query, cancellationToken);

        return ResolveResult(result);
    }

    [HttpGet("{id}")]
    [MustHavePermission(ActionCatalog.View, ResourceCatalog.Reservations)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetByIdReservationQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return ResolveResult(result);
    }

    [HttpPut("{id}")]
    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.Reservations)]
    public async Task<IActionResult> Update(Guid id, UpdateReservationRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateReservationCommand(
            id,
            request.PropertyId,
            request.GuestId,
            request.StartDate,
            request.EndDate,
            request.NumberOfGuests);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpPatch("{id}/status")]
    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.Reservations)]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateReservationStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateReservationStatusCommand(
            id,
            request.Status);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }

    [HttpDelete("{id}")]
    [MustHavePermission(ActionCatalog.Delete, ResourceCatalog.Reservations)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteReservationCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return ResolveResult(result);
    }
}

