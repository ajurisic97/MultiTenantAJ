using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.GetAll;

public record GetAllMaintenanceRequestQuery(
    Guid? PropertyId,
    MaintenanceRequestStatusEnum? Status,
    MaintenancePriorityEnum? Priority,
    DateTime? FromDate,
    DateTime? ToDate) : IRequest<ApplicationResult<List<MaintenanceRequestDto>>>;
