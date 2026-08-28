using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.GetById;

public record GetByIdGuestQuery(Guid Id) : IRequest<ApplicationResult<GuestDetailsDto>>;
