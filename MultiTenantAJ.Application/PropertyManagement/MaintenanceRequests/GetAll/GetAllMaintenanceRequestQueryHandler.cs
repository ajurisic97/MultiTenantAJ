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

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.GetAll;

public class GetAllMaintenanceRequestQueryHandler : IRequestHandler<GetAllMaintenanceRequestQuery, ApplicationResult<List<MaintenanceRequestDto>>>
{
    private readonly IRepository<MaintenanceRequest> _repository;

    public GetAllMaintenanceRequestQueryHandler(IRepository<MaintenanceRequest> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult<List<MaintenanceRequestDto>>> Handle(GetAllMaintenanceRequestQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.ListAsync(new SearchMaintenanceRequestSpec(
            request.PropertyId,
            request.Status,
            request.Priority,
            request.FromDate,
            request.ToDate),
            cancellationToken);

        var resultDto = result
            .Select(MaintenanceRequestMappings.ToDto)
            .ToList();

        return ApplicationResult<List<MaintenanceRequestDto>>.Success(resultDto);
    }
}
