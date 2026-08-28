using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Delete;

public record DeleteReservationCommand(Guid Id) : IRequest<ApplicationResult<Guid>>;
