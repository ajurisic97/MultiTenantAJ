using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Mappings.Identity;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Permissions.GetAll;

public class GetAllPermissionQueryHandler
    : IRequestHandler<GetAllPermissionQuery, ApplicationResult<List<PermissionDto>>>
{
    private readonly IRepository<Permission> _permissionRepository;

    public GetAllPermissionQueryHandler(IRepository<Permission> permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<ApplicationResult<List<PermissionDto>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.ListAsync(cancellationToken);

        var result = permissions
            .Select(x => PermissionMappings.ToDto(x))
            .ToList();

        return ApplicationResult<List<PermissionDto>>.Success(result);
    }
}
