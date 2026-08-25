using MediatR;
using MultiTenantAJ.Domain.Models.Catalog;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.GetAll;

public class GetProductsQueryHandler: IRequestHandler<GetProductsQuery, List<object>>
{
    private readonly IRepository<Product> _productRepository;

    public GetProductsQueryHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<object>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.ListAsync(cancellationToken);

        return [.. products
            .Select(x => (object)new
            {
                x.Id,
                x.Name,
                x.Price
            })];
    }
}
