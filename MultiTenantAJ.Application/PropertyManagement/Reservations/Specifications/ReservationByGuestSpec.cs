using Ardalis.Specification;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;

public class ReservationByGuestSpec : Specification<Reservation>
{
    public ReservationByGuestSpec(Guid guestId, bool activeOnly = false)
    {
        Query.Where(x => x.GuestId == guestId);

        if (activeOnly)
        {
            Query.Where(x =>
                x.Status == ReservationStatusEnum.Pending ||
                x.Status == ReservationStatusEnum.Confirmed);
        }
    }
}
