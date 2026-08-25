using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.GetById;

public record GetProductByIdQuery(Guid Id) : IRequest<object?>;
