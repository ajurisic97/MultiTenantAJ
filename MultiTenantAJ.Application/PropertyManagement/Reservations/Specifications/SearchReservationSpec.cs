using Ardalis.Specification;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;

public class SearchReservationSpec : Specification<Reservation>
{
    public SearchReservationSpec(Guid? propertyId, Guid? guestId, ReservationStatusEnum? status, DateTime? fromDate, DateTime? toDate)
    {
        Query
            .Include(x => x.Property)
            .Include(x => x.Guest);

        if (propertyId.HasValue)
        {
            Query.Where(x => x.PropertyId == propertyId.Value);
        }

        if (guestId.HasValue)
        {
            Query.Where(x => x.GuestId == guestId.Value);
        }

        if (status.HasValue)
        {
            Query.Where(x => x.Status == status.Value);
        }

        if (fromDate.HasValue)
        {
            Query.Where(x => x.StartDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            Query.Where(x => x.EndDate <= toDate.Value);
        }

        Query.OrderBy(x => x.StartDate);
    }
}
