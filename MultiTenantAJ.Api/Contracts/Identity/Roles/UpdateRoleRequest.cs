namespace MultiTenantAJ.Api.Contracts.Identity.Roles;
public record UpdateRoleRequest(
    string Name,
    string? Description);
