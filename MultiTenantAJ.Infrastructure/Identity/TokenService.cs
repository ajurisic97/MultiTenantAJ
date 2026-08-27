using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Identity;
using MultiTenantAJ.Domain.Authorization;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Identity;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public LoginUserDto GenerateToken(User user)
    {
        var accessTokenExpiryTime = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(MultitenancyConstants.TenantIdName, user.TenantId)
        };
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
            foreach (var permission in role.Permissions)
            {
                claims.Add(new Claim(Permissions.ClaimType, permission.Name));
            }
        }

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims,null, accessTokenExpiryTime, signingCredentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        var refreshToken = GenerateRefreshToken();
        user.SetRefreshToken(refreshToken, refreshTokenExpiryTime);

        return new LoginUserDto
        {
            Token = token,
            RefreshToken = refreshToken,
            AccessTokenExpiryTime = accessTokenExpiryTime,
            RefreshTokenExpiryTime = refreshTokenExpiryTime
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
