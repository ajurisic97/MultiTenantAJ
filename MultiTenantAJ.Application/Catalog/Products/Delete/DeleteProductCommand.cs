using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.Delete;

public record DeleteProductCommand(Guid Id) : IRequest<Guid?>;
