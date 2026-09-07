using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Identity.Users.Specifications;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
namespace MultiTenantAJ.Application.Identity.Users.Login;
public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApplicationResult<LoginUserDto>>
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    public LoginUserCommandHandler(IRepository<User> userRepository, IPasswordService passwordService, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationResult<LoginUserDto>> Handle(
        LoginUserCommand request, CancellationToken cancellationToken)
    {
        var specification = new UserByUsernameSpec(request.Username);

        var user = await _userRepository
            .FirstOrDefaultAsync(specification, cancellationToken);

        if (user == null)
        {
            return ApplicationResult<LoginUserDto>.Failure(
                ApplicationError.Unauthorized("Invalid username or password."));
        }

        if (!_passwordService.VerifyPassword(user, request.Password))
        {
            return ApplicationResult<LoginUserDto>.Failure(
                ApplicationError.Unauthorized("Invalid username or password."));
        }

        var loginUserDto = _tokenService.GenerateToken(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApplicationResult<LoginUserDto>.Success(loginUserDto);
    }
}
