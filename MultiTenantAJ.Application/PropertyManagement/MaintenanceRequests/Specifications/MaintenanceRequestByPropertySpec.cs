using Ardalis.Specification;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Specifications;

public class MaintenanceRequestByPropertySpec : Specification<MaintenanceRequest>
{
    public MaintenanceRequestByPropertySpec(Guid propertyId, bool activeOnly = false)
    {
        Query.Where(x => x.PropertyId == propertyId);

        if (activeOnly)
        {
            Query.Where(x =>
                x.Status == MaintenanceRequestStatusEnum.Open ||
                x.Status == MaintenanceRequestStatusEnum.InProgress);
        }
    }
}
