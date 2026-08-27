using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Create;

public record CreateUserCommand(
    string Username,
    string Password) : IRequest<ApplicationResult<Guid>>;
