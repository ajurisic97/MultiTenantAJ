using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Identity;
using MultiTenantAJ.Application.Mappings.Identity;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Identity.Users.GetAll;

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, ApplicationResult<List<UserDto>>>
{
    private readonly IRepository<User> _userRepository;

    public GetAllUserQueryHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationResult<List<UserDto>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.ListAsync(cancellationToken);

        var result = users.Select(x => UserMappings.ToDto(x)).ToList();

        return ApplicationResult<List<UserDto>>.Success(result);
    }
}
