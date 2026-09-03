namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Reservations;

public record CreateReservationRequest(
    Guid PropertyId,
    Guid GuestId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int NumberOfGuests);
