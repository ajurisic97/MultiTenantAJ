using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.GetById;

public record GetByIdProductQuery(Guid Id) : IRequest<ApplicationResult<object>>;
