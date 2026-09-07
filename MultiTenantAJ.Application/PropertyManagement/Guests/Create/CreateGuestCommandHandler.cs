using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.PropertyManagement.Guests.Create;

public class CreateGuestCommandHandler : IRequestHandler<CreateGuestCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<Guest> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGuestCommandHandler(IRepository<Guest> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
    {
        var result = Guest.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone);

        await _repository.AddAsync(result, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(result.Id);
    }
}
