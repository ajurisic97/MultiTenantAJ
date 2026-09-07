using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.GetAll;

public class GetAllMaintenanceRequestQueryValidator : AbstractValidator<GetAllMaintenanceRequestQuery>
{
    public GetAllMaintenanceRequestQueryValidator()
    {
        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
    }
}
