using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.Create;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Property> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePropertyCommandHandler(IRepository<Property> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = Property.Create(
            request.Name,
            request.Address,
            request.Description,
            request.NumberOfRooms,
            request.NumberOfBeds,
            request.MaximumCapacity);

        await _repository.AddAsync(property, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(property.Id);
    }
}
