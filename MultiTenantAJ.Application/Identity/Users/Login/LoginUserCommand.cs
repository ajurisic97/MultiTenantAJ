using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;

namespace MultiTenantAJ.Application.Identity.Users.Login;

public record LoginUserCommand(string Username, string Password) : IRequest<ApplicationResult<LoginUserDto>>;