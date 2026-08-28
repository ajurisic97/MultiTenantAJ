using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.Specifications;

public class GuestByIdSpec : SingleResultSpecification<Guest>
{
    public GuestByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
