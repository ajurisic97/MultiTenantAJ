using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.GetAll;

public record GetAllUserQuery(): IRequest<ApplicationResult<List<UserDto>>>;
