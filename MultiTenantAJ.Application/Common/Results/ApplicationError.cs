using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Common.Results;

public class ApplicationError
{
    public string Message { get; }
    public ApplicationErrorType Type { get; }

    private ApplicationError(string message, ApplicationErrorType type)
    {
        Message = message;
        Type = type;
    }

    public static ApplicationError Validation(string message)
    {
        return new ApplicationError(message, ApplicationErrorType.Validation);
    }

    public static ApplicationError NotFound(string message)
    {
        return new ApplicationError(message, ApplicationErrorType.NotFound);
    }

    public static ApplicationError Conflict(string message)
    {
        return new ApplicationError(message, ApplicationErrorType.Conflict);
    }

    public static ApplicationError Unauthorized(string message)
    {
        return new ApplicationError(message, ApplicationErrorType.Unauthorized);
    }

    public static ApplicationError Failure(string message)
    {
        return new ApplicationError(message, ApplicationErrorType.Failure);
    }

    public static ApplicationError Forbidden(string message)
    {
        return new ApplicationError(message, ApplicationErrorType.Forbidden);
    }
}
