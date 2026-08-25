using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using MultiTenantAJ.Domain.Repositories;
using MultiTenantAJ.Domain.Models.Catalog;


namespace MultiTenantAJ.Application.Catalog.Products.Create;

public class CreateProductCommandHandler: IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.Price);

        await _productRepository.AddAsync(product, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
