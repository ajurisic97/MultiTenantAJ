using FluentValidation;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.Create;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Property.NameMaxLength);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(Property.AddressMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(Property.DescriptionMaxLength);

        RuleFor(x => x.NumberOfRooms)
            .GreaterThan(0);

        RuleFor(x => x.NumberOfBeds)
            .GreaterThan(0);

        RuleFor(x => x.MaximumCapacity)
            .GreaterThan(0);
    }
}
