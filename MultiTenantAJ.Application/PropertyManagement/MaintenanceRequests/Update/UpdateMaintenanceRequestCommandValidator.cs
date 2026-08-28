using FluentValidation;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Update;

public class UpdateMaintenanceRequestCommandValidator : AbstractValidator<UpdateMaintenanceRequestCommand>
{
    public UpdateMaintenanceRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(MaintenanceRequest.DescriptionMaxLength);

        RuleFor(x => x.Priority)
            .IsInEnum();
    }
}
