using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
