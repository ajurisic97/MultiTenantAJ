using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.UpdateStatus;

public record UpdateMaintenanceRequestStatusCommand(Guid Id, MaintenanceRequestStatusEnum Status) : IRequest<ApplicationResult<Guid>>;
