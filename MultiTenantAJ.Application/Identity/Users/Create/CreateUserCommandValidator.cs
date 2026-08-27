using FluentValidation;
using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(User.UsernameMaxLength);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(User.PasswordMinLength)
            .MaximumLength(User.PasswordMaxLength);
    }
}
