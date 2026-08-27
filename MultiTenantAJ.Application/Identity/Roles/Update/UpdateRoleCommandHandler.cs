using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Identity.Roles.Specifications;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.Update;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleCommandHandler(IRepository<Role> roleRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id,cancellationToken);

        if (role == null)
        {
            return ApplicationResult<Guid>.Failure(
                ApplicationError.NotFound("Role was not found."));
        }

        var existingRole = await _roleRepository.SingleOrDefaultAsync(new RoleByNameSpec(request.Name, request.Id), cancellationToken);

        if (existingRole != null)
        {
            return ApplicationResult<Guid>.Failure(
                ApplicationError.Conflict(
                    "Role with specified name already exists."));
        }

        role.Update(request.Name, request.Description);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(role.Id);
    }
}
