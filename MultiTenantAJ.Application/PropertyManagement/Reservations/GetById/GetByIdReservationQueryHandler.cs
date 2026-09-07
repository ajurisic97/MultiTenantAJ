using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Application.Mappings.PropertyManagement;
using MultiTenantAJ.Application.PropertyManagement.Reservations.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.GetById;

public class GetByIdReservationQueryHandler : IRequestHandler<GetByIdReservationQuery, ApplicationResult<ReservationDetailsDto>>
{
    private readonly IRepository<Reservation> _repository;

    public GetByIdReservationQueryHandler(IRepository<Reservation> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult<ReservationDetailsDto>> Handle(GetByIdReservationQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.SingleOrDefaultAsync(new ReservationByIdSpec(request.Id,true), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<ReservationDetailsDto>.Failure(ApplicationError.NotFound("Reservation was not found."));
        }

        var resultDto = ReservationMappings.ToDetailsDto(result);

        return ApplicationResult<ReservationDetailsDto>.Success(resultDto);
    }
}
