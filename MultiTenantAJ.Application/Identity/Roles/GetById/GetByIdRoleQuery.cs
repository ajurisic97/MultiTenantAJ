using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.GetById;

public record GetByIdRoleQuery(Guid Id) : IRequest<ApplicationResult<RoleDetailsDto>>;