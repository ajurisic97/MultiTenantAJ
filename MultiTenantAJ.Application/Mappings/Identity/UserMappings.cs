using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Domain.Models.Identity;
namespace MultiTenantAJ.Application.Mappings.Identity;
public static class UserMappings
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };
    }

    public static UserDetailsDto ToDetailsDto(User user)
    {
        return new UserDetailsDto
        {
            Id = user.Id,
            Username = user.Username,
            Roles = user.Roles
                .Select(x => RoleMappings.ToDto(x))
                .ToList()
        };
    }
}
