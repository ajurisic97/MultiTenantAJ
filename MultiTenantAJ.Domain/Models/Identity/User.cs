using MultiTenantAJ.Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Models.Identity;
public class User : IMustHaveTenant
{
    public Guid Id { get; private set; }

    public string TenantId { get; set; } = default!;

    public string Username { get; private set; } = default!;

    public string PasswordHash { get; private set; } = default!;

    public string? RefreshToken { get; private set; }

    public DateTime? RefreshTokenExpiryTime { get; private set; }

    public ICollection<Role> Roles { get; set; } = [];

    public User()
    {
    }

    private User(Guid id, string username, string passwordHash)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
    }

    public static User Create(string username,string passwordHash)
    {
        return new User(
            Guid.NewGuid(),
            username,
            passwordHash);
    }

    public void UpdateUsername(string username)
    {
        Username = username;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void SetRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    public const int UsernameMaxLength = 50;
    public const int PasswordHashMaxLength = 200;
}
