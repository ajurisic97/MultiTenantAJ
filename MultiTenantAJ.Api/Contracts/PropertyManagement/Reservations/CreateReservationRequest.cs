namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Reservations;

public record CreateReservationRequest(
    Guid PropertyId,
    Guid GuestId,
    DateTime StartDate,
    DateTime EndDate,
    int NumberOfGuests);
