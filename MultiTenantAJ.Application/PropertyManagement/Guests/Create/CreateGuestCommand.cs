using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.Create;
public record CreateGuestCommand(
    string FirstName,
    string LastName,
    string? Email,
    string? Phone) : IRequest<ApplicationResult<Guid>>;
