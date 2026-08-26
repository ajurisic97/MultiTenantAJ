using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Identity;

public class JwtSettings
{
    public string SecretKey { get; set; } 
    public string Issuer { get; set; } 
    public string Audience { get; set; } 
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}
