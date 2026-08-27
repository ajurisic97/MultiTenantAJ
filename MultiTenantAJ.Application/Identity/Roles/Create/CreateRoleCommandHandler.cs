using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Identity.Roles.Specifications;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Roles.Create;

public class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(
        IRepository<Role> roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var existingRole = await _roleRepository.SingleOrDefaultAsync(new RoleByNameSpec(request.Name), cancellationToken);

        if (existingRole != null)
        {
            return ApplicationResult<Guid>.Failure(
                ApplicationError.Conflict("Role with specified name already exists."));
        }

        var role = Role.Create(request.Name,request.Description);

        await _roleRepository.AddAsync(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(role.Id);
    }
}
