using MediatR;
using MultiTenantAJ.Application.Catalog.Products.Specifications;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Models.Catalog;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Catalog.Products.Delete;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.SingleOrDefaultAsync(
            new ProductByIdSpec(request.Id),
            cancellationToken);

        if (product==null)
        {
            return ApplicationResult<Guid>.Failure(
                ApplicationError.NotFound("Product was not found."));
        }

        await _productRepository.DeleteAsync(
            product,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(product.Id);
    }
}
