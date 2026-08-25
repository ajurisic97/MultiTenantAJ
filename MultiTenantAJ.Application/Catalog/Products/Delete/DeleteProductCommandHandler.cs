using MediatR;
using MultiTenantAJ.Application.Catalog.Products.Specifications;
using MultiTenantAJ.Domain.Models.Catalog;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.Delete;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Guid?>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.SingleOrDefaultAsync(
            new ProductByIdSpec(request.Id),
            cancellationToken);

        if (product is null)
        {
            return null;
        }

        await _productRepository.DeleteAsync(
            product,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
