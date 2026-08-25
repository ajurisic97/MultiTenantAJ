using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.GetAll;

public record GetProductsQuery(): IRequest<List<object>>;
