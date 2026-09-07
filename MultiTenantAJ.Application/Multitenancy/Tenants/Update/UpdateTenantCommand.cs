using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Multitenancy;

namespace MultiTenantAJ.Application.Multitenancy.Tenants.Update;
public record UpdateTenantCommand(
    string Id,
    bool IsActive,
    bool MaintenanceEnabled) : IRequest<ApplicationResult<TenantDto>>;
