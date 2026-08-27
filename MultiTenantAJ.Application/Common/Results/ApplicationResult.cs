using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Common.Results;

public class ApplicationResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public ApplicationError? Error { get; }

    private ApplicationResult(bool isSuccess, T? value, ApplicationError? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static ApplicationResult<T> Success(T value)
    {
        return new ApplicationResult<T>(true, value, null);
    }

    public static ApplicationResult<T> Failure(ApplicationError error)
    {
        return new ApplicationResult<T>(false, default, error);
    }
}
