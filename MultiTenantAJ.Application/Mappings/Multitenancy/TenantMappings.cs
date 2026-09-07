using MultiTenantAJ.Application.Dto.Multitenancy;
using MultiTenantAJ.Domain.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Mappings.Multitenancy;

public static class TenantMappings
{
    public static TenantDto ToDto(Tenant tenant)
    {
        return new TenantDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            ApiKey = tenant.ApiKey,
            IsActive = tenant.IsActive
        };
    }
}
