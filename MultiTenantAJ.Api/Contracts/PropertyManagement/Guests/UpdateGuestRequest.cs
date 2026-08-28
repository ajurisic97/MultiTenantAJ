namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Guests;

public record UpdateGuestRequest(
    string FirstName,
    string LastName,
    string? Email,
    string? Phone);
