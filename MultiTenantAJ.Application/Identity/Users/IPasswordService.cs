using MultiTenantAJ.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users;

public interface IPasswordService
{
    bool VerifyPassword(User user, string password);
    string HashPassword(User user, string password);
}
