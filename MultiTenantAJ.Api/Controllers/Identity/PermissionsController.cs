using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Api.Authorization;
using MultiTenantAJ.Application.Identity.Permissions.GetAll;
using MultiTenantAJ.Domain.Authorization;

namespace MultiTenantAJ.Api.Controllers.Identity;

[Tags("Identity - Permissions")]
[Route("api/[controller]")]
public class PermissionsController : ApiControllerBase
{
    private readonly ISender _sender;

    public PermissionsController(ISender sender)
    {
        _sender = sender;
    }

    [MustHavePermission(ActionCatalog.Search, ResourceCatalog.Permissions)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllPermissionQuery();
        var result = await _sender.Send(query);

        return ResolveResult(result);
    }
}
