using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Multitenancy;
using MultiTenantAJ.Application.Mappings.Multitenancy;

namespace MultiTenantAJ.Application.Multitenancy.Tenants.Create;

public class CreateTenantCommandHandler  : IRequestHandler<CreateTenantCommand, ApplicationResult<TenantDto>>
{
    private readonly ITenantService _tenantService;

    public CreateTenantCommandHandler(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public async Task<ApplicationResult<TenantDto>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var result = await _tenantService.CreateTenantAsync(request.Id, request.Name, request.ConnectionString, cancellationToken);
        if (!result.IsSuccess)
        {
            return ApplicationResult<TenantDto>.Failure(result.Error!);
        }

        var tenantDto = TenantMappings.ToDto(result.Value!);

        return ApplicationResult<TenantDto>.Success(tenantDto);
    }
}
