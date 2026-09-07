using MultiTenantAJ.Domain.Enums;

namespace MultiTenantAJ.Api.Contracts.PropertyManagement.Reservations;

public record UpdateReservationStatusRequest(ReservationStatusEnum Status);
