using Microsoft.AspNetCore.Mvc;
using MultiTenantAJ.Application.Common.Results;

namespace MultiTenantAJ.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ResolveResult<T>(ApplicationResult<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        IActionResult response;

        switch (result.Error.Type)
        {
            case ApplicationErrorType.Validation:
                response = BadRequest(result.Error.Message);
                break;

            case ApplicationErrorType.Unauthorized:
                response = Unauthorized(result.Error.Message);
                break;

            case ApplicationErrorType.NotFound:
                response = NotFound(result.Error.Message);
                break;

            case ApplicationErrorType.Conflict:
                response = Conflict(result.Error.Message);
                break;

            case ApplicationErrorType.Failure:
                response = StatusCode(
                    StatusCodes.Status500InternalServerError,
                    result.Error.Message);
                break;

            case ApplicationErrorType.Forbidden:
                response = StatusCode(
                    StatusCodes.Status403Forbidden,
                    result.Error.Message);
                break;

            default:
                response = StatusCode(StatusCodes.Status500InternalServerError);
                break;
        }

        return response;
    }
}
