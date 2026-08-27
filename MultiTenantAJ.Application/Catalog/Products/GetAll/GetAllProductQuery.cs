using MediatR;
using MultiTenantAJ.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.GetAll;

public record GetAllProductQuery() : IRequest<ApplicationResult<List<object>>>;