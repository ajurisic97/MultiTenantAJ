using FluentValidation;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Create;

public class CreateMaintenanceRequestCommandValidator : AbstractValidator<CreateMaintenanceRequestCommand>
{
    public CreateMaintenanceRequestCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(MaintenanceRequest.DescriptionMaxLength);

        RuleFor(x => x.Priority)
            .IsInEnum();
    }
}
