using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Identity.Users.Specifications;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.RefreshToken;
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApplicationResult<LoginUserDto>>
{
    private readonly IRepository<User> _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
        IRepository<User> userRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<LoginUserDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.SingleOrDefaultAsync(new UserByRefreshTokenSpec(request.RefreshToken), cancellationToken);

        if (user == null)
        {
            return ApplicationResult<LoginUserDto>.Failure(ApplicationError.Unauthorized("Invalid refresh token."));
        }

        if (user.RefreshTokenExpiryTime == null ||  user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return ApplicationResult<LoginUserDto>.Failure(ApplicationError.Unauthorized("Refresh token has expired."));
        }

        var loginUserDto = _tokenService.GenerateToken(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<LoginUserDto>.Success(loginUserDto);
    }
}
