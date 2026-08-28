using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Specifications;
using MultiTenantAJ.Application.PropertyManagement.Properties.Specifications;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.Delete;

public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Property> _propertyRepository;
    private readonly IRepository<Reservation> _reservationRepository;
    private readonly IRepository<MaintenanceRequest> _maintenanceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePropertyCommandHandler(IRepository<Property> propertyRepository, IRepository<Reservation> reservationRepository, IRepository<MaintenanceRequest> maintenanceRequestRepository, IUnitOfWork unitOfWork)
    {
        _propertyRepository = propertyRepository;
        _reservationRepository = reservationRepository;
        _maintenanceRequestRepository = maintenanceRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
    {
        var result = await _propertyRepository.SingleOrDefaultAsync(new PropertyByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Property was not found."));
        }

        var reservationCount = await _reservationRepository.CountAsync(new ReservationByPropertySpec(result.Id), cancellationToken);

        if (reservationCount > 0)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Property cannot be deleted because it has reservations."));
        }

        var maintenanceRequestCount = await _maintenanceRequestRepository.CountAsync(new MaintenanceRequestByPropertySpec(result.Id), cancellationToken);

        if (maintenanceRequestCount > 0)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Property cannot be deleted because it has maintenance requests."));
        }

        await _propertyRepository.DeleteAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
