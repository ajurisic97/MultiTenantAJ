using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.Update;
public record UpdateProductCommand(
    Guid Id,
    string Name,
    decimal Price) : IRequest<Guid?>;
