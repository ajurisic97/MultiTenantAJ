using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Logout;

public record LogoutUserCommand(Guid UserId) : IRequest<ApplicationResult<Guid>>;
