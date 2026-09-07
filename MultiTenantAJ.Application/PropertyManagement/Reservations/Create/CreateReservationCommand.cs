using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Create;

public record CreateReservationCommand(
    Guid PropertyId,
    Guid GuestId,
    DateTime StartDate,
    DateTime EndDate,
    int NumberOfGuests) : IRequest<ApplicationResult<Guid>>;
