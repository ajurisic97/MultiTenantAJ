using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;

public class ReservationByIdSpec : SingleResultSpecification<Reservation>
{
    public ReservationByIdSpec(Guid id, bool includeDetails = false)
    {
        Query.Where(x => x.Id == id);

        if (includeDetails)
        {
            Query
                .Include(x => x.Property)
                .Include(x => x.Guest);
        }
    }
}
