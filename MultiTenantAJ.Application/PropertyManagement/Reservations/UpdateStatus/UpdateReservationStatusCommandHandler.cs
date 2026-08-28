using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.UpdateStatus;

public class UpdateReservationStatusCommandHandler : IRequestHandler<UpdateReservationStatusCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReservationStatusCommandHandler(IRepository<Reservation> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(UpdateReservationStatusCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.SingleOrDefaultAsync(new ReservationByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Reservation was not found."));
        }

        switch (result.Status)
        {
            case ReservationStatusEnum.Pending:
                if (request.Status == ReservationStatusEnum.Confirmed)
                {
                    result.Confirm();
                }
                else if (request.Status == ReservationStatusEnum.Cancelled)
                {
                    result.Cancel();
                }
                else
                {
                    return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Invalid reservation status transition."));
                }

                break;

            case ReservationStatusEnum.Confirmed:
                if (request.Status == ReservationStatusEnum.Cancelled)
                {
                    result.Cancel();
                }
                else if (request.Status == ReservationStatusEnum.Completed)
                {
                    result.Complete();
                }
                else
                {
                    return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Invalid reservation status transition."));
                }

                break;

            case ReservationStatusEnum.Cancelled:
            case ReservationStatusEnum.Completed:
                return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Reservation status can no longer be changed."));
        }

        await _repository.UpdateAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
