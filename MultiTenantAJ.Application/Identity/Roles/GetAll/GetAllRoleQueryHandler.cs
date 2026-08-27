using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Mappings.Identity;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.GetAll;

public class GetAllRoleQueryHandler: IRequestHandler<GetAllRoleQuery, ApplicationResult<List<RoleDto>>>
{
    private readonly IRepository<Role> _roleRepository;

    public GetAllRoleQueryHandler(IRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ApplicationResult<List<RoleDto>>> Handle(
        GetAllRoleQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.ListAsync(cancellationToken);

        var result = roles
            .Select(x => RoleMappings.ToDto(x))
            .ToList();

        return ApplicationResult<List<RoleDto>>.Success(result);
    }
}
