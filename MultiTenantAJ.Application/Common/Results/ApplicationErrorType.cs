using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Common.Results;

public enum ApplicationErrorType
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Failure
}
