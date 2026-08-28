using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Delete;

public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReservationCommandHandler(IRepository<Reservation> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.SingleOrDefaultAsync(new ReservationByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Reservation was not found."));
        }

        await _repository.DeleteAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
