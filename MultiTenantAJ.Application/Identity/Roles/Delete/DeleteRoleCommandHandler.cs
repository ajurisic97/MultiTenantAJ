using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Identity.Roles.Specifications;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;

namespace MultiTenantAJ.Application.Identity.Roles.Delete;

public class DeleteRoleCommandHandler
    : IRequestHandler<DeleteRoleCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(IRepository<Role> roleRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.SingleOrDefaultAsync( new RoleByIdSpec(request.Id,false,true), cancellationToken);

        if (role == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Role was not found."));
        }

        var hasUsers = role.Users.Count > 0;

        if (hasUsers)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Conflict( "Role cannot be deleted because it is assigned to one or more users."));
        }

        await _roleRepository.DeleteAsync(role,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(role.Id);
    }
}
