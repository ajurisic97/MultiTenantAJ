using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Identity.Users.Specifications;
using MultiTenantAJ.Application.Mappings.Identity;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.GetById;

public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, ApplicationResult<UserDetailsDto>>
{
    private readonly IRepository<User> _userRepository;

    public GetByIdUserQueryHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationResult<UserDetailsDto>> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.SingleOrDefaultAsync(new UserByIdSpec(request.Id, true), cancellationToken);

        if (user == null)
        {
            return ApplicationResult<UserDetailsDto>.Failure(ApplicationError.NotFound("User was not found."));
        }

        var result = UserMappings.ToDetailsDto(user);

        return ApplicationResult<UserDetailsDto>.Success(result);
    }
}
