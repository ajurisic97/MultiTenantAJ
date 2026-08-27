using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Identity.Users.Specifications;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IRepository<User> userRepository, IPasswordService passwordService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(UpdateUserCommand request,CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user == null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.NotFound("User was not found."));
        }

        var existingUser = await _userRepository.SingleOrDefaultAsync(new UserByUsernameSpec(request.Username, request.Id), cancellationToken);

        if (existingUser != null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("User with specified username already exists."));
        }

        user.UpdateUsername(request.Username);

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var passwordIsSame = _passwordService.VerifyPassword(user, request.Password);

            if (!passwordIsSame)
            {
                var passwordHash = _passwordService.HashPassword(user, request.Password);
                user.UpdatePassword(passwordHash);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(user.Id);
    }
}
