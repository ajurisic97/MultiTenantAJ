using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.Update;

public record UpdatePropertyCommand(
    Guid Id,
    string Name,
    string Address,
    string? Description,
    int NumberOfRooms,
    int NumberOfBeds,
    int MaximumCapacity,
    bool IsActive) : IRequest<ApplicationResult<Guid>>;
