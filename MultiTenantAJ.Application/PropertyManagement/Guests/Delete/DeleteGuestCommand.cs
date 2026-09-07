using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.Delete;

public record DeleteGuestCommand(Guid Id) : IRequest<ApplicationResult<Guid>>;
