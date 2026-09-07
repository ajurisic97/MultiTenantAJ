using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Delete;

public record DeleteMaintenanceRequestCommand(Guid Id) : IRequest<ApplicationResult<Guid>>;
