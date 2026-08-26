using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Contracts.Identity.Users;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Identity.Users.Login;
using MultiTenantAJ.Domain.Multitenancy;

namespace MultiTenantAJ.Api.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserDto>> Login(
        [FromHeader(Name = MultitenancyConstants.TenantIdName)] string tenant,
        LoginUserRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(request.Username, request.Password);
        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
}
