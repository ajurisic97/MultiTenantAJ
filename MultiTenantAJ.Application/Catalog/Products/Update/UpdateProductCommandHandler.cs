using MediatR;
using MultiTenantAJ.Application.Catalog.Products.Specifications;
using MultiTenantAJ.Domain.Models.Catalog;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.Update;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Guid?>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.SingleOrDefaultAsync(
            new ProductByIdSpec(request.Id),
            cancellationToken);

        if (product is null)
        {
            return null;
        }

        product.Update(
            request.Name,
            request.Price);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
