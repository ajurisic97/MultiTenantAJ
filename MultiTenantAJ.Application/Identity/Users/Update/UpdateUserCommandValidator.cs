using FluentValidation;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(User.UsernameMaxLength);

        RuleFor(x => x.Password)
            .MinimumLength(6)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}
