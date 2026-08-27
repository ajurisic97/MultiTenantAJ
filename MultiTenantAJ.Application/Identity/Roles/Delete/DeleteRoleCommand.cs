using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.Delete;

public record DeleteRoleCommand(Guid Id) : IRequest<ApplicationResult<Guid>>;
