using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.GetAll;

public record GetAllPropertyQuery(
    string? Name,
    string? Address,
    bool? IsActive,
    int? MinimumCapacity) : IRequest<ApplicationResult<List<PropertyDto>>>;
