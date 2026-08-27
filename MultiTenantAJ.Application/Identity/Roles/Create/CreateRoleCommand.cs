using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.Create;

public record CreateRoleCommand(string Name, string? Description) : IRequest<ApplicationResult<Guid>>;
