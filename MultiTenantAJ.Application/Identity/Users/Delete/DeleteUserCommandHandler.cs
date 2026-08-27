using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IRepository<User> userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id,cancellationToken);
        if (user == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("User was not found."));
        }

        await _userRepository.DeleteAsync(user,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(user.Id);
    }
}
