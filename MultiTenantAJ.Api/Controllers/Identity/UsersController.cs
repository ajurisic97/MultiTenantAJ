using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Api.Contracts.Identity.Users;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Identity.Users.Create;
using MultiTenantAJ.Application.Identity.Users.Delete;
using MultiTenantAJ.Application.Identity.Users.GetAll;
using MultiTenantAJ.Application.Identity.Users.GetById;
using MultiTenantAJ.Application.Identity.Users.Login;
using MultiTenantAJ.Application.Identity.Users.Logout;
using MultiTenantAJ.Application.Identity.Users.RefreshToken;
using MultiTenantAJ.Application.Identity.Users.Update;
using MultiTenantAJ.Application.Identity.Users.UpdateUserRoles;
using MultiTenantAJ.Domain.Authorization;
using MultiTenantAJ.Domain.Multitenancy;
using System.Security.Claims;

namespace MultiTenantAJ.Api.Controllers.Identity;

[Route("api/[controller]")]
public class UsersController : ApiControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromHeader(Name = MultitenancyConstants.TenantIdName)] string tenant,
        LoginUserRequest request)
    {
        var command = new LoginUserCommand(request.Username, request.Password);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
    [FromHeader(Name = MultitenancyConstants.TenantIdName)] string tenant,
    RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var command = new LogoutUserCommand(userId);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Search, ResourceCatalog.Users)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllUserQuery();
        var result = await _sender.Send(query);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.View, ResourceCatalog.Users)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdUserQuery(id);
        var result = await _sender.Send(query);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Create, ResourceCatalog.Users)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var command = new CreateUserCommand(request.Username, request.Password);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Update, ResourceCatalog.Users)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(
            id,
            request.Username,
            request.Password);

        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [MustHavePermission(ActionCatalog.Delete, ResourceCatalog.Users)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteUserCommand(id);
        var result = await _sender.Send(command);

        return ResolveResult(result);
    }

    [MustHavePermission(
    ActionCatalog.Update,
    ResourceCatalog.UserRoles)]
    [HttpPut("{id}/roles")]
    public async Task<IActionResult> UpdateRoles(Guid id, UpdateUserRolesRequest request)
    {
        var command = new UpdateUserRolesCommand(id, request.RoleIds);

        var result = await _sender.Send(command);

        return ResolveResult(result);
    }
}
