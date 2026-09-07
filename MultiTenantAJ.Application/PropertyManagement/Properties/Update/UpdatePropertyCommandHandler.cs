using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.Properties.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.Update;

public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Property> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePropertyCommandHandler(IRepository<Property> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.SingleOrDefaultAsync(new PropertyByIdSpec(request.Id), cancellationToken);

        if (result == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Property was not found."));
        }

        result.Update(
            request.Name,
            request.Address,
            request.Description,
            request.NumberOfRooms,
            request.NumberOfBeds,
            request.MaximumCapacity);
        if (request.IsActive)
        {
            result.Activate();
        }
        else
        {
            result.Deactivate();
        }
        await _repository.UpdateAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
