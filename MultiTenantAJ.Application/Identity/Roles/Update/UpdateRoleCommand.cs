using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.Update;

public record UpdateRoleCommand(Guid Id,string Name, string? Description) : IRequest<ApplicationResult<Guid>>;
