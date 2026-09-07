namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Guests;

public record CreateGuestRequest(
    string FirstName,
    string LastName,
    string? Email,
    string? Phone);
