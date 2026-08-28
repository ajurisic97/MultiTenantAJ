using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Update;

public record UpdateReservationCommand(
    Guid Id,
    Guid PropertyId,
    Guid GuestId,
    DateTime StartDate,
    DateTime EndDate,
    int NumberOfGuests) : IRequest<ApplicationResult<Guid>>;
