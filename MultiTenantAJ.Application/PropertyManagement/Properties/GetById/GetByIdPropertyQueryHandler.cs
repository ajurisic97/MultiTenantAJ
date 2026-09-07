using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Application.Mappings.PropertyManagement;
using MultiTenantAJ.Application.PropertyManagement.MaintenanceRequests.Specifications;
using MultiTenantAJ.Application.PropertyManagement.Properties.Specifications;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.GetById;

public class GetByIdPropertyQueryHandler : IRequestHandler<GetByIdPropertyQuery, ApplicationResult<PropertyDetailsDto>>
{
    private readonly IRepository<Property> _propertyRepository;
    private readonly IRepository<Reservation> _reservationRepository;
    private readonly IRepository<MaintenanceRequest> _maintenanceRequestRepository;

    public GetByIdPropertyQueryHandler(IRepository<Property> propertyRepository,IRepository<Reservation> reservationRepository, IRepository<MaintenanceRequest> maintenanceRequestRepository)
    {
        _propertyRepository = propertyRepository;
        _reservationRepository = reservationRepository;
        _maintenanceRequestRepository = maintenanceRequestRepository;
    }

    public async Task<ApplicationResult<PropertyDetailsDto>> Handle(GetByIdPropertyQuery request, CancellationToken cancellationToken)
    {
        var result = await _propertyRepository.SingleOrDefaultAsync(new PropertyByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<PropertyDetailsDto>.Failure(ApplicationError.NotFound("Property was not found."));
        }

        var reservationCount = await _reservationRepository.CountAsync(new ReservationByPropertySpec(result.Id), cancellationToken);

        var activeReservationCount = await _reservationRepository.CountAsync(new ReservationByPropertySpec(result.Id, true),
            cancellationToken);

        var activeMaintenanceRequestCount = await _maintenanceRequestRepository.CountAsync(new MaintenanceRequestByPropertySpec(result.Id, true),
            cancellationToken);

        var resultDto = PropertyMappings.ToDetailsDto(
            result,
            reservationCount,
            activeReservationCount,
            activeMaintenanceRequestCount);

        return ApplicationResult<PropertyDetailsDto>.Success(resultDto);
    }
}
