using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Application.Mappings.PropertyManagement;
using MultiTenantAJ.Application.PropertyManagement.Guests.Specifications;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.GetById;

public class GetByIdGuestQueryHandler : IRequestHandler<GetByIdGuestQuery, ApplicationResult<GuestDetailsDto>>
{
    private readonly IRepository<Guest> _guestRepository;
    private readonly IRepository<Reservation> _reservationRepository;

    public GetByIdGuestQueryHandler(IRepository<Guest> guestRepository, IRepository<Reservation> reservationRepository)
    {
        _guestRepository = guestRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<ApplicationResult<GuestDetailsDto>> Handle(GetByIdGuestQuery request, CancellationToken cancellationToken)
    {
        var result = await _guestRepository.SingleOrDefaultAsync(new GuestByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<GuestDetailsDto>.Failure(ApplicationError.NotFound("Guest was not found."));
        }

        var reservationCount = await _reservationRepository.CountAsync(new ReservationByGuestSpec(result.Id), cancellationToken);

        var activeReservationCount = await _reservationRepository.CountAsync(new ReservationByGuestSpec(result.Id, true),
            cancellationToken);

        var resultDto = GuestMappings.ToDetailsDto(
            result,
            reservationCount,
            activeReservationCount);

        return ApplicationResult<GuestDetailsDto>.Success(resultDto);
    }
}
