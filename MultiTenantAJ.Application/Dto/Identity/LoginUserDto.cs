using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Dto.Identity;

public class LoginUserDto
{
    public string Token { get; set; } 
    public string RefreshToken { get; set; } 
    public DateTime AccessTokenExpiryTime { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
