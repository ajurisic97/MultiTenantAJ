using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Identity.Roles.Specifications;
using MultiTenantAJ.Application.Mappings.Identity;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.GetById;

public class GetByIdRoleQueryHandler : IRequestHandler<GetByIdRoleQuery, ApplicationResult<RoleDetailsDto>>
{
    private readonly IRepository<Role> _roleRepository;

    public GetByIdRoleQueryHandler(IRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ApplicationResult<RoleDetailsDto>> Handle(GetByIdRoleQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.SingleOrDefaultAsync(
            new RoleByIdSpec(request.Id,true),
            cancellationToken);

        if (role == null)
        {
            return ApplicationResult<RoleDetailsDto>.Failure(
                ApplicationError.NotFound("Role was not found."));
        }

        var result = RoleMappings.ToDetailsDto(role);

        return ApplicationResult<RoleDetailsDto>.Success(result);
    }
}
