using MediatR;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Dto.Multitenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Application.Multitenancy.Tenants.Create;

public record CreateTenantCommand(string Id, string Name, string? ConnectionString) : IRequest<ApplicationResult<TenantDto>>;
