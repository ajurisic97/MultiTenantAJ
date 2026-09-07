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
            var failureResult = ApplicationResult<T>.Failure(ApplicationError.Failure("An unexpected error occurred."));

            return StatusCode(StatusCodes.Status500InternalServerError,failureResult);   
        }

        IActionResult response;

        switch (result.Error.Type)
        {
            case ApplicationErrorType.Validation:
                response = BadRequest(result);
                break;

            case ApplicationErrorType.Unauthorized:
                response = Unauthorized(result);
                break;

            case ApplicationErrorType.NotFound:
                response = NotFound(result);
                break;

            case ApplicationErrorType.Conflict:
                response = Conflict(result);
                break;

            case ApplicationErrorType.Failure:
                response = StatusCode(
                    StatusCodes.Status500InternalServerError,
                    result);
                break;

            case ApplicationErrorType.Forbidden:
                response = StatusCode(
                    StatusCodes.Status403Forbidden,
                    result);
                break;

            default:
                response = StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApplicationResult<T>.Failure(
                        ApplicationError.Failure(
                            "An unexpected error occurred.")));
                break;
        }

        return response;
    }
}
