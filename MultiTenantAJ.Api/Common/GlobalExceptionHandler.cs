using Microsoft.AspNetCore.Diagnostics;
using MultiTenantAJ.Application.Common.Results;

namespace MultiTenantAJ.Api.Common;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unexpected error occurred.");

        var result = ApplicationResult<object>.Failure(
            ApplicationError.Failure("An unexpected error occurred."));

        httpContext.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            result,
            cancellationToken);

        return true;
    }
}
