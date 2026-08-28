namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Reservations;

public record UpdateReservationRequest(
    Guid PropertyId,
    Guid GuestId,
    DateTime StartDate,
    DateTime EndDate,
    int NumberOfGuests);
