using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Identity.Roles.Specifications;
using MultiTenantAJ.Application.Identity.Users.Specifications;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.UpdateUserRoles;

public class UpdateUserRolesCommandHandler
    : IRequestHandler<UpdateUserRolesCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserRolesCommandHandler(IRepository<User> userRepository,IRepository<Role> roleRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.SingleOrDefaultAsync(new UserByIdSpec(request.UserId, true), cancellationToken);

        if (user == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("User was not found."));
        }

        var requestedRoleIds = request.RoleIds.Distinct().ToList();

        var currentRoleIds = user.Roles.Select(x => x.Id).ToHashSet();

        if (currentRoleIds.SetEquals(requestedRoleIds))
        {
            return ApplicationResult<Guid>.Success(user.Id);
        }

        var roles = await _roleRepository.ListAsync(new RoleByIdsSpec(requestedRoleIds), cancellationToken);

        if (roles.Count != requestedRoleIds.Count)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("One or more roles were not found."));
        }

        var rolesToRemove = user.Roles.Where(x => !requestedRoleIds.Contains(x.Id)).ToList();

        foreach (var role in rolesToRemove)
        {
            user.Roles.Remove(role);
        }

        var rolesToAdd = roles.Where(x => !currentRoleIds.Contains(x.Id)).ToList();

        foreach (var role in rolesToAdd)
        {
            user.Roles.Add(role);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(user.Id);
    }
}
