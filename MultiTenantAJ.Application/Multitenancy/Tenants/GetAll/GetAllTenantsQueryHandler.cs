using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Multitenancy;
using MultiTenantAJ.Application.Mappings.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Multitenancy.Tenants.GetAll;

public class GetAllTenantsQueryHandler : IRequestHandler<GetAllTenantsQuery, ApplicationResult<List<TenantDto>>>
{
    private readonly ITenantService _tenantService;

    public GetAllTenantsQueryHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public async Task<ApplicationResult<List<TenantDto>>> Handle(
        GetAllTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var tenants = await _tenantService.GetAllTenantsAsync(cancellationToken);

        var result = tenants
            .Select(TenantMappings.ToDto)
            .ToList();

        return ApplicationResult<List<TenantDto>>.Success(result);
    }
}
