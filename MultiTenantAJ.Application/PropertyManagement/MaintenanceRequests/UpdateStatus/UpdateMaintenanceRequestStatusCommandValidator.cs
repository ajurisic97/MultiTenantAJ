using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.UpdateStatus;

public class UpdateMaintenanceRequestStatusCommandValidator : AbstractValidator<UpdateMaintenanceRequestStatusCommand>
{
    public UpdateMaintenanceRequestStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
