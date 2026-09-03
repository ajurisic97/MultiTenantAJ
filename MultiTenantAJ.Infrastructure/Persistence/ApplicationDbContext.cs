using Microsoft.EntityFrameworkCore;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Models.PropertyManagement;
using MultiTenantAJ.Domain.Multitenancy;
namespace MultiTenantAJ.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentTenantService _currentTenantService;
    private string? CurrentTenantConnectionString => _currentTenantService.ConnectionString;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentTenantService currentTenantService) : base(options)
    {
        _currentTenantService = currentTenantService;
    }

    #region Identity
    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    #endregion

    #region PropertyManagement
    public DbSet<Property> Properties => Set<Property>();

    public DbSet<Guest> Guests => Set<Guest>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    public DbSet<MaintenanceRequest> MaintenanceRequests => Set<MaintenanceRequest>();

    #endregion
    public string? CurrentTenantId => _currentTenantService.TenantId;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly, 
            type => type.Namespace != null &&
            type.Namespace.Contains("Persistence.Configurations"));

        ApplyTenantQueryFilters(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrWhiteSpace(CurrentTenantConnectionString))
        {
            optionsBuilder.UseNpgsql(CurrentTenantConnectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    public override int SaveChanges()
    {
        return SaveChanges(true);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        HandleTenantData();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, 
        CancellationToken cancellationToken = default)
    {
        HandleTenantData();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(true, cancellationToken);
    }

    private void ApplyTenantQueryEntityFilter<TEntity>(ModelBuilder modelBuilder) 
        where TEntity : class, IMustHaveTenant
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(x => x.TenantId == CurrentTenantId);
    }

    private void ApplyTenantQueryFilters(ModelBuilder modelBuilder)
    {
        #region PropertyManagement

        ApplyTenantQueryEntityFilter<Guest>(modelBuilder);
        ApplyTenantQueryEntityFilter<MaintenanceRequest>(modelBuilder);
        ApplyTenantQueryEntityFilter<Property>(modelBuilder);
        ApplyTenantQueryEntityFilter<Reservation>(modelBuilder);

        #endregion
        #region Identity

        ApplyTenantQueryEntityFilter<User>(modelBuilder);
        ApplyTenantQueryEntityFilter<Role>(modelBuilder);
        #endregion

    }

    private void HandleTenantData()
    {
        var currentTenantId = CurrentTenantId;
        if (string.IsNullOrWhiteSpace(currentTenantId))
        {
            throw new InvalidOperationException("Current tenant is not available.");
        }

        var entries = ChangeTracker.Entries<IMustHaveTenant>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.TenantId = currentTenantId;
            }

            if ((entry.State == EntityState.Modified || entry.State == EntityState.Deleted) 
                && entry.Entity.TenantId != currentTenantId)
            {
                throw new InvalidOperationException("Cross-tenant data modification is not allowed.");
            }
        }
    }
}