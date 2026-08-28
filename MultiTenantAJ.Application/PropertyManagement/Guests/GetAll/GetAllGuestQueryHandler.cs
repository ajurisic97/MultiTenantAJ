using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Application.Mappings.PropertyManagement;
using MultiTenantAJ.Application.PropertyManagement.Guests.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.GetAll;

public class GetAllGuestQueryHandler : IRequestHandler<GetAllGuestQuery, ApplicationResult<IReadOnlyCollection<GuestDto>>>
{
    private readonly IRepository<Guest> _repository;

    public GetAllGuestQueryHandler(IRepository<Guest> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult<IReadOnlyCollection<GuestDto>>> Handle(GetAllGuestQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.ListAsync(new SearchGuestSpec(request.FirstName, request.LastName, request.Email, request.Phone),
            cancellationToken);

        var resultDto = result
            .Select(GuestMappings.ToDto)
            .ToList();

        return ApplicationResult<IReadOnlyCollection<GuestDto>>.Success(resultDto);
    }
}
