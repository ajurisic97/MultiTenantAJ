using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Models.Catalog;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.GetAll;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, ApplicationResult<List<object>>>
{
    private readonly IRepository<Product> _productRepository;

    public GetAllProductQueryHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ApplicationResult<List<object>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.ListAsync(cancellationToken);

        var result = products
            .Select(x => (object)new
            {
                x.Id,
                x.Name,
                x.Price
            }).ToList();

        return ApplicationResult<List<object>>.Success(result);

    }
}
