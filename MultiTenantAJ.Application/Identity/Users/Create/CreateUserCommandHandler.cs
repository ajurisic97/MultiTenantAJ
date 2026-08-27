using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Identity.Users.Specifications;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApplicationResult<Guid>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IRepository<User> userRepository, IPasswordService passwordService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.SingleOrDefaultAsync(new UserByUsernameSpec(request.Username,false), cancellationToken);
        if (existingUser != null)
        {
            return ApplicationResult<Guid>.Failure(ApplicationError.Conflict("User with specified username already exists."));
        }

        var user = User.Create(
            request.Username,
            string.Empty);

        var passwordHash = _passwordService.HashPassword(user, request.Password);

        user.UpdatePassword(passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Guid>.Success(user.Id);
    }
}
