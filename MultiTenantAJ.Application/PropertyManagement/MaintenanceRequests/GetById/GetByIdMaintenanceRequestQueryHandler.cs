using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Application.Mappings.PropertyManagement;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.GetById;

public class GetByIdMaintenanceRequestQueryHandler : IRequestHandler<GetByIdMaintenanceRequestQuery, ApplicationResult<MaintenanceRequestDetailsDto>>
{
    private readonly IRepository<MaintenanceRequest> _repository;

    public GetByIdMaintenanceRequestQueryHandler(IRepository<MaintenanceRequest> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult<MaintenanceRequestDetailsDto>> Handle(GetByIdMaintenanceRequestQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.SingleOrDefaultAsync(new MaintenanceRequestByIdSpec(request.Id, true), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<MaintenanceRequestDetailsDto>.Failure(ApplicationError.NotFound("Maintenance request was not found."));
        }

        var resultDto = MaintenanceRequestMappings.ToDetailsDto(result);

        return ApplicationResult<MaintenanceRequestDetailsDto>.Success(resultDto);
    }
}
