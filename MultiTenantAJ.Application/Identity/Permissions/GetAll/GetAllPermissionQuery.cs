using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;

namespace MultiTenantAJ.Application.Identity.Permissions.GetAll;

public record GetAllPermissionQuery() : IRequest<ApplicationResult<List<PermissionDto>>>;
