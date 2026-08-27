namespace MultiTenantAJ.Api.Contracts.Identity.Users;

public record CreateUserRequest(
    string Username,
    string Password);
