using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.Guests.Specifications;
using MultiTenantAJ.Application.PropertyManagement.Properties.Specifications;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.Update;

public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Reservation> _reservationRepository;
    private readonly IRepository<Property> _propertyRepository;
    private readonly IRepository<Guest> _guestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReservationCommandHandler(IRepository<Reservation> reservationRepository, IRepository<Property> propertyRepository, IRepository<Guest> guestRepository, IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _propertyRepository = propertyRepository;
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var result = await _reservationRepository.SingleOrDefaultAsync(new ReservationByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Reservation was not found."));
        }

        var property = await _propertyRepository.SingleOrDefaultAsync(new PropertyByIdSpec(request.PropertyId), cancellationToken);

        if (property == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Property was not found."));
        }

        if (!property.IsActive)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Property is inactive."));
        }
        if (request.NumberOfGuests > property.MaximumCapacity)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Number of guests exceeds the maximum property capacity."));
        }

        var guest = await _guestRepository.SingleOrDefaultAsync(new GuestByIdSpec(request.GuestId), cancellationToken);

        if (guest == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Guest was not found."));
        }

        var hasOverlappingReservation = await _reservationRepository.AnyAsync(
            new OverlappingReservationSpec(
                request.PropertyId,
                request.StartDate,
                request.EndDate,
                result.Id),
            cancellationToken);

        if (hasOverlappingReservation)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("Property already has a reservation for the selected period."));
        }

        result.Update(
            request.PropertyId,
            request.GuestId,
            request.StartDate,
            request.EndDate,
            request.NumberOfGuests);

        await _reservationRepository.UpdateAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
