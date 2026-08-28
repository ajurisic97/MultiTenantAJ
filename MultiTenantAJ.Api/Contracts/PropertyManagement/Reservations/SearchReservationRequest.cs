using MultiTenantAJ.Domain.Enums;

namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Reservations;

public record SearchReservationRequest(
    Guid? PropertyId,
    Guid? GuestId,
    ReservationStatusEnum? Status,
    DateTime? FromDate,
    DateTime? ToDate);
