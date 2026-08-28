using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.GetAll;

public record GetAllReservationQuery(
    Guid? PropertyId,
    Guid? GuestId,
    ReservationStatusEnum? Status,
    DateTime? FromDate,
    DateTime? ToDate) : IRequest<ApplicationResult<List<ReservationDto>>>;
