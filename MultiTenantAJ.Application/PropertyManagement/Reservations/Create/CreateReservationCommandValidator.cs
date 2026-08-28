using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Create;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty();

        RuleFor(x => x.GuestId)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.NumberOfGuests)
            .GreaterThan(0);
    }
}
