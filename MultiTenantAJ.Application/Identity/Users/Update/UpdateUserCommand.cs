using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Update;

public record UpdateUserCommand(
    Guid Id,
    string Username,
    string? Password) : IRequest<ApplicationResult<Guid>>;
