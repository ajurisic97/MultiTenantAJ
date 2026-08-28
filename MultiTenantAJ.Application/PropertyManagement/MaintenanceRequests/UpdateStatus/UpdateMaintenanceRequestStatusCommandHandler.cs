using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Specifications;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.UpdateStatus;

public class UpdateMaintenanceRequestStatusCommandHandler : IRequestHandler<UpdateMaintenanceRequestStatusCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<MaintenanceRequest> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMaintenanceRequestStatusCommandHandler(IRepository<MaintenanceRequest> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(UpdateMaintenanceRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.SingleOrDefaultAsync(new MaintenanceRequestByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Maintenance request was not found."));
        }

        switch (result.Status)
        {
            case MaintenanceRequestStatusEnum.Open:
                if (request.Status == MaintenanceRequestStatusEnum.InProgress)
                {
                    result.StartProgress();
                }
                else if (request.Status == MaintenanceRequestStatusEnum.Cancelled)
                {
                    result.Cancel();
                }
                else
                {
                    return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Invalid maintenance request status transition."));
                }

                break;

            case MaintenanceRequestStatusEnum.InProgress:
                if (request.Status == MaintenanceRequestStatusEnum.Resolved)
                {
                    result.Resolve();
                }
                else if (request.Status == MaintenanceRequestStatusEnum.Cancelled)
                {
                    result.Cancel();
                }
                else
                {
                    return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Invalid maintenance request status transition."));
                }

                break;

            case MaintenanceRequestStatusEnum.Resolved:
                if (request.Status == MaintenanceRequestStatusEnum.InProgress)
                {
                    result.StartProgress();
                }
                else if (request.Status == MaintenanceRequestStatusEnum.Closed)
                {
                    result.Close();
                }
                else
                {
                    return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Invalid maintenance request status transition."));
                }

                break;

            case MaintenanceRequestStatusEnum.Closed:
            case MaintenanceRequestStatusEnum.Cancelled:
                return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Maintenance request status can no longer be changed."));
        }

        await _repository.UpdateAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
