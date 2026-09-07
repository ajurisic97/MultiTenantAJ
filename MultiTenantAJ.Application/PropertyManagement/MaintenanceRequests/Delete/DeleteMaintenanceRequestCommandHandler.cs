using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Delete;

public class DeleteMaintenanceRequestCommandHandler : IRequestHandler<DeleteMaintenanceRequestCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<MaintenanceRequest> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMaintenanceRequestCommandHandler(IRepository<MaintenanceRequest> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(DeleteMaintenanceRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.SingleOrDefaultAsync(new MaintenanceRequestByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Maintenance request was not found."));
        }

        await _repository.DeleteAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
