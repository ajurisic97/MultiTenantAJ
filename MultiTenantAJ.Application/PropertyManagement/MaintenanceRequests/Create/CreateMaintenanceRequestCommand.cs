using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Create;

public record CreateMaintenanceRequestCommand(
    Guid PropertyId,
    string Description,
    MaintenancePriorityEnum Priority) : IRequest<ApplicationResult<Guid>>;
