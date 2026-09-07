using Microsoft.EntityFrameworkCore;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Enums;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;
using MultiTenantAJ.Infrastructure.Seeder.DataHelpers;

namespace MultiTenantAJ.Infrastructure.Seeder;

public class DataSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICurrentTenantService _currentTenantService;

    public DataSeeder(
        ApplicationDbContext dbContext,
        ICurrentTenantService currentTenantService)
    {
        _dbContext = dbContext;
        _currentTenantService = currentTenantService;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = _currentTenantService.TenantId;

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            throw new InvalidOperationException("Current tenant is not available.");
        }

        if (tenantId == MultitenancyConstants.RootTenantId)
        {
            return;
        }

        var dataExists = await _dbContext.Properties.AnyAsync(cancellationToken);

        if (dataExists)
        {
            return;
        }

        var properties = DemoPropertyGenerator.Create(tenantId);
        var guests = DemoGuestGenerator.Create(tenantId);
        var reservations = DemoReservationGenerator.Create(properties, guests);
        var maintenanceRequests = DemoMaintenanceRequestGenerator.Create(properties);

        await _dbContext.Properties.AddRangeAsync(properties, cancellationToken);
        await _dbContext.Guests.AddRangeAsync(guests, cancellationToken);
        await _dbContext.Reservations.AddRangeAsync(reservations, cancellationToken);
        await _dbContext.MaintenanceRequests.AddRangeAsync(
            maintenanceRequests,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
