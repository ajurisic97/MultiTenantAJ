using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Delete;

public record DeleteUserCommand(Guid Id) : IRequest<ApplicationResult<Guid>>;
