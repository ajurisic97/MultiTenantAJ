using MediatR;
using MultiTenantAJ.Application.Catalog.Products.Specifications;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Models.Catalog;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.GetById;

public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, ApplicationResult<object>>
{
    private readonly IRepository<Product> _productRepository;

    public GetByIdProductQueryHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ApplicationResult<object>> Handle(GetByIdProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null)
        {
            return ApplicationResult<object>.Failure(
                ApplicationError.NotFound("Product was not found."));
        }

        var result = new
        {
            product.Id,
            product.Name,
            product.Price
        };

        return ApplicationResult<object>.Success(result);
    }
}
