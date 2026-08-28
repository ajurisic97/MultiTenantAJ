using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Multitenancy.Tenants.Create;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();
    }
}
