using Ardalis.Specification;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;

public class OverlappingReservationSpec : Specification<Reservation>
{
    public OverlappingReservationSpec(Guid propertyId, DateTime startDate, DateTime endDate, Guid? excludeReservationId = null)
    {
        Query.Where(x =>
            x.PropertyId == propertyId &&
            (x.Status != ReservationStatusEnum.Cancelled) &&
            x.StartDate < endDate &&
            x.EndDate > startDate);

        if (excludeReservationId.HasValue)
        {
            Query.Where(x => x.Id != excludeReservationId.Value);
        }
    }
}
