using Ardalis.Specification;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;

public class ReservationByPropertySpec : Specification<Reservation>
{
    public ReservationByPropertySpec(Guid propertyId, bool activeOnly = false)
    {
        Query.Where(x => x.PropertyId == propertyId);
        if (activeOnly)
        {
            Query.Where(x =>
            (x.Status == ReservationStatusEnum.Pending ||
             x.Status == ReservationStatusEnum.Confirmed));
        }
    }
}