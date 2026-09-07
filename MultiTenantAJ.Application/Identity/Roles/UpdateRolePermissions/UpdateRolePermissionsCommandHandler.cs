using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Identity.Permissions.Specifications;
using MultiTenantAJ.Application.Identity.Roles.Specifications;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Authorization;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Domain.Repositories;

namespace MultiTenantAJ.Application.Identity.Roles.UpdateRolePermissions;

public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRolePermissionsCommandHandler(
        IRepository<Role> roleRepository,
        IRepository<Permission> permissionRepository,
        ICurrentTenantService currentTenantService,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _currentTenantService = currentTenantService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle( UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.SingleOrDefaultAsync(new RoleByIdSpec(request.RoleId, includePermissions: true),
            cancellationToken);

        if (role == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Role was not found."));
        }

        var requestedPermissionIds = request.PermissionIds.Distinct().ToList();
        var currentPermissionIds = role.Permissions.Select(x => x.Id).ToHashSet();

        var permissions = await _permissionRepository.ListAsync(new PermissionByIdsSpec(requestedPermissionIds), cancellationToken);

        if (permissions.Count != requestedPermissionIds.Count)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("One or more permissions were not found."));
        }

        var hasMaintenancePermissions = permissions.Any(x => x.Name.StartsWith($"Permissions.{ResourceCatalog.MaintenanceRequests}."));
        if (!_currentTenantService.MaintenanceEnabled && hasMaintenancePermissions)
        {
            return ApplicationResult<Guid>.Failure(
                ApplicationError.Forbidden(
                    "Maintenance permissions cannot be assigned because maintenance is disabled for this tenant."));
        }

        var hasRootOnlyPermissions = permissions.Any(permission =>
            Domain.Authorization.Permissions.All.Any(x =>
                x.Name == permission.Name &&
                x.IsRootOnly));

        if (_currentTenantService.TenantId != MultitenancyConstants.RootTenantId && hasRootOnlyPermissions)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Forbidden("Root-only permissions cannot be assigned by this tenant."));
        }

        if (currentPermissionIds.SetEquals(requestedPermissionIds))
        {
            return ApplicationResult<Guid>.Success(role.Id);
        }

        var permissionsToRemove = role.Permissions
            .Where(x => !requestedPermissionIds.Contains(x.Id))
            .ToList();

        foreach (var permission in permissionsToRemove)
        {
            role.Permissions.Remove(permission);
        }

        var permissionsToAdd = permissions
            .Where(x => !currentPermissionIds.Contains(x.Id))
            .ToList();

        foreach (var permission in permissionsToAdd)
        {
            role.Permissions.Add(permission);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(role.Id);
    }
}
