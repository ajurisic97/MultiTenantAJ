using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.GetAll;

public class GetAllReservationQueryValidator : AbstractValidator<GetAllReservationQuery>
{
    public GetAllReservationQueryValidator()
    {
        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
    }
}
