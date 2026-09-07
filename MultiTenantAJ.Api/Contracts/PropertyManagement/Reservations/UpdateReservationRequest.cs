namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Reservations;

public record UpdateReservationRequest(
    Guid PropertyId,
    Guid GuestId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int NumberOfGuests);
