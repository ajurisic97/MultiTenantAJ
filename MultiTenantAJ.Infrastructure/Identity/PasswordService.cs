using Microsoft.AspNetCore.Identity;
using MultiTenantAJ.Application.Identity.Users;
using MultiTenantAJ.Domain.Models.Identity;

namespace MultiTenantAJ.Infrastructure.Identity;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public bool VerifyPassword(User user, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            password);

        return result != PasswordVerificationResult.Failed;
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }
}
