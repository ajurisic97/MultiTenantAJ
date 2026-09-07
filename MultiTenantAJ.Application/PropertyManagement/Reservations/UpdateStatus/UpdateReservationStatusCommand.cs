using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.UpdateStatus;

public record UpdateReservationStatusCommand(Guid Id, ReservationStatusEnum Status) : IRequest<ApplicationResult<Guid>>;
