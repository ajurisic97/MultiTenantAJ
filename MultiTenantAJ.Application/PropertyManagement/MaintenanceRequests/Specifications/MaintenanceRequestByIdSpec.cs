using Ardalis.Specification;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Specifications;

public class MaintenanceRequestByIdSpec : SingleResultSpecification<MaintenanceRequest>
{
    public MaintenanceRequestByIdSpec(Guid id, bool includeProperty = false)
    {
        Query.Where(x => x.Id == id);

        if (includeProperty)
        {
            Query.Include(x => x.Property);
        }
    }
}
