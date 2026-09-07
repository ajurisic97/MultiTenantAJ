using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Mappings.Identity;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Authorization;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Permissions.GetAll;

public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, ApplicationResult<List<PermissionDto>>>
{
    private readonly IRepository<Permission> _permissionRepository;
    private readonly ICurrentTenantService _currentTenantService;

    public GetAllPermissionQueryHandler(IRepository<Permission> permissionRepository, ICurrentTenantService currentTenantService)
    {
        _permissionRepository = permissionRepository;
        _currentTenantService = currentTenantService;
    }

    public async Task<ApplicationResult<List<PermissionDto>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.ListAsync(cancellationToken);
        if (!_currentTenantService.MaintenanceEnabled)
        {
            permissions = permissions
                .Where(x => !x.Name.StartsWith($"Permissions.{ResourceCatalog.MaintenanceRequests}."))
                .ToList();
        }
        var result = permissions
            .Select(x => PermissionMappings.ToDto(x))
            .ToList();

        return ApplicationResult<List<PermissionDto>>.Success(result);
    }
}
