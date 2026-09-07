using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.GetById;

public record GetByIdPropertyQuery(Guid Id) : IRequest<ApplicationResult<PropertyDetailsDto>>;
