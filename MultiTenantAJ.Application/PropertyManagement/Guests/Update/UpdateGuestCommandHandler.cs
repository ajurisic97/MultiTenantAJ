using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.PropertyManagement.Guests.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.Update;

public class UpdateGuestCommandHandler : IRequestHandler<UpdateGuestCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Guest> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGuestCommandHandler(IRepository<Guest> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.SingleOrDefaultAsync(new GuestByIdSpec(request.Id), cancellationToken);
        if (result == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("Guest was not found."));
        }

        result.Update(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone);

        await _repository.UpdateAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
