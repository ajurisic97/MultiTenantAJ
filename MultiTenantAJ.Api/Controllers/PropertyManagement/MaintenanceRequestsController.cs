using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MultiTenantAJ.Api.Controllers.PropertyManagement;

[Route("api/[controller]")]
public class MaintenanceRequestsController : ApiControllerBase
{
    private readonly ISender _sender;

    public MaintenanceRequestsController(ISender sender)
    {
        _sender = sender;
    }
}
