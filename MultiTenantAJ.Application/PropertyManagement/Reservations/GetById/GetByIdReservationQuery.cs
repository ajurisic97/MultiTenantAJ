using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.GetById;
public record GetByIdReservationQuery(Guid Id) : IRequest<ApplicationResult<ReservationDetailsDto>>;