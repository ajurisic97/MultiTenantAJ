using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Logout;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutUserCommandHandler(
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return ApplicationResult<Guid>.Failure(
                ApplicationError.Unauthorized("User was not found."));
        }

        if (user.RefreshToken == null)
        {
            return ApplicationResult<Guid>.Success(user.Id);
        }

        user.ClearRefreshToken();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(user.Id);
    }
}
