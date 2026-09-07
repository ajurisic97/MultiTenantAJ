using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Multitenancy;
namespace MultiTenantAJ.Application.Multitenancy.Tenants.GetAll;

public record GetAllTenantsQuery() : IRequest<ApplicationResult<List<TenantDto>>>;
