using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.Guests.Specifications;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.Delete;

public class DeleteGuestCommandHandler : IRequestHandler<DeleteGuestCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Guest> _guestRepository;
    private readonly IRepository<Reservation> _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGuestCommandHandler(IRepository<Guest> guestRepository, IRepository<Reservation> reservationRepository, IUnitOfWork unitOfWork)
    {
        _guestRepository = guestRepository;
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
    {
        var result = await _guestRepository.SingleOrDefaultAsync(new GuestByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Guest was not found."));
        }

        var hasReservations = await _reservationRepository.AnyAsync(new ReservationByGuestSpec(result.Id), cancellationToken);

        if (hasReservations)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Guest cannot be deleted because they have reservations."));
        }

        await _guestRepository.DeleteAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
