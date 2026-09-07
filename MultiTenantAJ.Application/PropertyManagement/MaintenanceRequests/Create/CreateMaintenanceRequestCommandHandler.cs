using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.Properties.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Create;

public class CreateMaintenanceRequestCommandHandler : IRequestHandler<CreateMaintenanceRequestCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<MaintenanceRequest> _maintenanceRequestRepository;
    private readonly IRepository<Property> _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMaintenanceRequestCommandHandler(IRepository<MaintenanceRequest> maintenanceRequestRepository, IRepository<Property> propertyRepository, IUnitOfWork unitOfWork)
    {
        _maintenanceRequestRepository = maintenanceRequestRepository;
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(CreateMaintenanceRequestCommand request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.SingleOrDefaultAsync(new PropertyByIdSpec(request.PropertyId), cancellationToken);

        if (property == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Property was not found."));
        }

        var result = MaintenanceRequest.Create(
            request.PropertyId,
            request.Description,
            request.Priority);

        await _maintenanceRequestRepository.AddAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
