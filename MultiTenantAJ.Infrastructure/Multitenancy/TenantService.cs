using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantAJ.Application.Common.Results;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;
using MultiTenantAJ.Infrastructure.Seeder;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Multitenancy;

public class TenantService : ITenantService
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly IServiceProvider _serviceProvider;

    public TenantService(TenantDbContext tenantDbContext, IServiceProvider serviceProvider)
    {
        _tenantDbContext = tenantDbContext;
        _serviceProvider = serviceProvider;
    }

    public async Task<ApplicationResult<Tenant>> CreateTenantAsync(string id, string name, string? connectionString, CancellationToken cancellationToken = default)
    {
        var tenantExists = await _tenantDbContext.Tenants.AnyAsync(x => x.Id == id, cancellationToken);

        if (tenantExists)
        {
            return ApplicationResult<Tenant>.Failure(ApplicationError.Conflict("Tenant already exists."));
        }

        var tenant = new Tenant
        {
            Id = id,
            ApiKey = Guid.NewGuid(),
            Name = name,
            ConnectionString = connectionString,
            IsActive = false
        };

        await _tenantDbContext.Tenants.AddAsync(tenant, cancellationToken);

        await _tenantDbContext.SaveChangesAsync(cancellationToken);

        await using var scope = _serviceProvider.CreateAsyncScope();
        var currentTenantService = scope.ServiceProvider.GetRequiredService<CurrentTenantService>();
        currentTenantService.SetTenant(tenant);

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await applicationDbContext.Database.MigrateAsync(cancellationToken);

        var identitySeeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
        await identitySeeder.SeedAsync(cancellationToken);

        tenant.IsActive = true;
        await _tenantDbContext.SaveChangesAsync(cancellationToken);

        return ApplicationResult<Tenant>.Success(tenant);
    }
}
