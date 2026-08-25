using MediatR;
using MultiTenantAJ.Application.Catalog.Products.Specifications;
using MultiTenantAJ.Domain.Models.Catalog;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.GetById;

public class GetProductByIdQueryHandler: IRequestHandler<GetProductByIdQuery, object?>
{
    private readonly IRepository<Product> _productRepository;

    public GetProductByIdQueryHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<object?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.SingleOrDefaultAsync(new ProductByIdSpec(request.Id), cancellationToken);

        if (product is null)
        {
            return null;
        }

        return new
        {
            product.Id,
            product.Name,
            product.Price
        };
    }
}
