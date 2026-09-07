using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.Update;

public record UpdateGuestCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string? Phone) : IRequest<ApplicationResult<Guid>>;
