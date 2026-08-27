using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.Create;

public record CreateProductCommand(
    string Name,
    decimal Price) : IRequest<ApplicationResult<Guid>>;
