using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.PropertyManagement;
using MultiTenantAJ.Application.Mappings.PropertyManagement;
using MultiTenantAJ.Application.PropertyManagement.Properties.Specifications;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Repositories;

namespace MultiTenantAJ.Application.PropertyManagement.Properties.GetAll;

public class GetAllPropertyQueryHandler : IRequestHandler<GetAllPropertyQuery, ApplicationResult<IReadOnlyCollection<PropertyDto>>>
{
    private readonly IRepository<Property> _repository;

    public GetAllPropertyQueryHandler(IRepository<Property> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult<IReadOnlyCollection<PropertyDto>>> Handle(GetAllPropertyQuery request, CancellationToken cancellationToken)
    {
        var specification = new SearchPropertySpec(
            request.Name,
            request.Address,
            request.IsActive,
            request.MinimumCapacity);
        var result = await _repository.ListAsync(specification, cancellationToken);

        var resultDto = result
                .Select(PropertyMappings.ToDto)
                .ToList();

        return ApplicationResult<IReadOnlyCollection<PropertyDto>>.Success(resultDto);
    }
}
