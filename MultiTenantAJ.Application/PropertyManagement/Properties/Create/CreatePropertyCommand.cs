using MediatR;
using MultiTenantAJ.Application.Common.Results;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.Create;
public record CreatePropertyCommand(
    string Name,
    string Address,
    string? Description,
    int NumberOfRooms,
    int NumberOfBeds,
    int MaximumCapacity) : IRequest<ApplicationResult<Guid>>;
