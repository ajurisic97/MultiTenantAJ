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

namespace MultiTenantAJ.Application.PropertyManagement.Reservations.GetAll;

public class GetAllReservationQueryHandler :
    IRequestHandler<GetAllReservationQuery, ApplicationResult<List<ReservationDto>>>
{
    private readonly IRepository<Reservation> _repository;

    public GetAllReservationQueryHandler(IRepository<Reservation> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult<List<ReservationDto>>> Handle(
        GetAllReservationQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.ListAsync(new SearchReservationSpec(
            request.PropertyId,
            request.GuestId,
            request.Status,
            request.FromDate,
            request.ToDate),
            cancellationToken);

        var resultDto = result
            .Select(ReservationMappings.ToDto)
            .ToList();

        return ApplicationResult<List<ReservationDto>>.Success(resultDto);
    }
}
