using MediatR;
using MultiTenantAJ.Application.Common.Results;

namespace MultiTenantAJ.Application.Identity.Roles.UpdateRolePermissions;
public record UpdateRolePermissionsCommand(Guid RoleId, List<int> PermissionIds) : IRequest<ApplicationResult<Guid>>;
