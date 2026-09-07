using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.Delete;

public record DeletePropertyCommand(Guid Id) : IRequest<ApplicationResult<Guid>>;
