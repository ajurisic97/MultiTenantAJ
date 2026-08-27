namespace MultiTenantAJ.Api.Contracts.Identity.Users;

public record UpdateUserRolesRequest(List<Guid> RoleIds);
