using FluentValidation;
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
        if (exception is ValidationException validationException)
        {
            var message = string.Join(
                " ",
                validationException.Errors.Select(x => x.ErrorMessage));

            var validationResult = ApplicationResult<object>.Failure(
                ApplicationError.Validation(message));

            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                validationResult,
                cancellationToken);

            return true;
        }

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
