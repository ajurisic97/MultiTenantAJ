using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.UpdateUserRoles;

public record UpdateUserRolesCommand(
    Guid UserId,
    List<Guid> RoleIds) : IRequest<ApplicationResult<Guid>>;
