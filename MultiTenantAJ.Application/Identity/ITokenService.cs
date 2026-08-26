using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Domain.Models.Identity;

namespace MultiTenantAJ.Application.Identity;

public interface ITokenService
{
    LoginUserDto GenerateToken(User user);
}
