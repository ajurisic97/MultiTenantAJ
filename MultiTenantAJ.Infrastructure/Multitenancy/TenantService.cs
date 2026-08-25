using Microsoft.Extensions.DependencyInjection;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure.Multitenancy;

public class TenantService
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly IServiceProvider _serviceProvider;

    public TenantService(
        TenantDbContext tenantDbContext,
        IServiceProvider serviceProvider)
    {
        _tenantDbContext = tenantDbContext;
        _serviceProvider = serviceProvider;
    }

    public async Task<Tenant> CreateTenantAsync(
        string id,
        string name,
        string connectionString,
        CancellationToken cancellationToken = default)
    {
        var tenantExists = await _tenantDbContext.Tenants
            .AnyAsync(x => x.Id == id, cancellationToken);

        if (tenantExists)
        {
            throw new InvalidOperationException(
                "Tenant already exists.");
        }

        var tenant = new Tenant
        {
            Id = id,
            ApiKey = Guid.NewGuid(),
            Name = name,
            ConnectionString = connectionString,
            IsActive = true
        };

        await _tenantDbContext.Tenants.AddAsync(
            tenant,
            cancellationToken);

        await _tenantDbContext.SaveChangesAsync(
            cancellationToken);

        using var scope = _serviceProvider.CreateScope();

        var applicationDbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        applicationDbContext.Database
            .SetConnectionString(connectionString);

        await applicationDbContext.Database
            .MigrateAsync(cancellationToken);

        return tenant;
    }
}
