using FluentValidation;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.Create;

public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
{
    public CreateGuestCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(Guest.FirstNameMaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(Guest.LastNameMaxLength);

        RuleFor(x => x.Email)
            .MaximumLength(Guest.EmailMaxLength)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(Guest.PhoneMaxLength);
    }
}
