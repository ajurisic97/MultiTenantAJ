using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.GetAll;

public record GetAllGuestQuery(string? FirstName, string? LastName, string? Email, string? Phone) : IRequest<ApplicationResult<List<GuestDto>>>;
