using Ardalis.Specification;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Specifications;

public class SearchMaintenanceRequestSpec : Specification<MaintenanceRequest>
{
    public SearchMaintenanceRequestSpec(Guid? propertyId, MaintenanceRequestStatusEnum? status, MaintenancePriorityEnum? priority, DateTime? fromDate, DateTime? toDate)
    {
        Query.Include(x => x.Property);

        if (propertyId.HasValue)
        {
            Query.Where(x => x.PropertyId == propertyId.Value);
        }

        if (status.HasValue)
        {
            Query.Where(x => x.Status == status.Value);
        }

        if (priority.HasValue)
        {
            Query.Where(x => x.Priority == priority.Value);
        }

        if (fromDate.HasValue)
        {
            Query.Where(x => x.CreatedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            Query.Where(x => x.CreatedAt <= toDate.Value);
        }

        Query.OrderByDescending(x => x.CreatedAt);
    }
}
