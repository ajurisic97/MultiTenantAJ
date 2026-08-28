using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MultiTenantAJ.Api.Controllers.PropertyManagement;

[Route("api/[controller]")]
public class ReservationsController : ApiControllerBase
{
    private readonly ISender _sender;

    public ReservationsController(ISender sender)
    {
        _sender = sender;
    }
}

