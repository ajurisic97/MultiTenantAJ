using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Multitenancy;
using MultiTenantAJ.Application.Mappings.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Multitenancy.Tenants.Update;

public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, ApplicationResult<TenantDto>>
{
    private readonly ITenantService _tenantService;

    public UpdateTenantCommandHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public async Task<ApplicationResult<TenantDto>> Handle(
        UpdateTenantCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _tenantService.UpdateTenantAsync(
            request.Id,
            request.IsActive,
            request.MaintenanceEnabled,
            cancellationToken);

        if (!result.IsSuccess)
        {
            return ApplicationResult<TenantDto>.Failure(result.Error!);
        }

        var tenantDto = TenantMappings.ToDto(result.Value!);

        return ApplicationResult<TenantDto>.Success(tenantDto);
    }
}
