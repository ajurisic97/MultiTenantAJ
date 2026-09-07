namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Guests;

public record SearchGuestRequest(
    string? FirstName,
    string? LastName,
    string? Email,
    string? Phone);
