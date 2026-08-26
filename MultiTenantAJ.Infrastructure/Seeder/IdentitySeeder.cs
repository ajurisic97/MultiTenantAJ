using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Authorization;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;

namespace MultiTenantAJ.Infrastructure.Seeder;

public class IdentitySeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IPasswordHasher<User> _passwordHasher;
    public IdentitySeeder(ApplicationDbContext dbContext, ICurrentTenantService currentTenantService, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _currentTenantService = currentTenantService;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedPermissionsAsync(cancellationToken);
        await SeedRolesAsync(cancellationToken);
        await SeedUsersAsync(cancellationToken);
    }

    #region Permission helper
    public async Task SeedPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var existingPermissionNames = await _dbContext.Permissions
            .Select(x => x.Name)
            .ToListAsync(cancellationToken);

        var missingPermissions = Permissions.All
            .Where(x => !existingPermissionNames.Contains(x.Name))
            .Select(x => new Permission
            {
                Name = x.Name
            })
            .ToList();

        if (missingPermissions.Count == 0)
        {
            return;
        }

        await _dbContext.Permissions.AddRangeAsync(missingPermissions, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion
    #region Role and RolePermission helper
    public async Task SeedRolesAsync(CancellationToken cancellationToken = default)
    {
        var currentTenantId = _currentTenantService.TenantId;

        if (string.IsNullOrWhiteSpace(currentTenantId))
        {
            throw new InvalidOperationException("Current tenant is not available.");
        }

        var permissions = await _dbContext.Permissions.ToListAsync(cancellationToken);

        if (currentTenantId == MultitenancyConstants.RootTenantId)
        {
            await SeedSuperAdminRoleAsync(permissions, cancellationToken);
            return;
        }

        var tenantPermissionNames = Permissions.All
            .Where(x => !x.IsRootOnly)
            .Select(x => x.Name)
            .ToHashSet();

        var applicationPermissions = permissions
            .Where(x => tenantPermissionNames.Contains(x.Name))
            .ToList();

        await CreateRoleIfMissingAsync(RoleCatalog.Admin, applicationPermissions, cancellationToken);

        await CreateRoleIfMissingAsync(RoleCatalog.User, [], cancellationToken);
    }

    private async Task SeedSuperAdminRoleAsync(IReadOnlyCollection<Permission> permissions, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == RoleCatalog.SuperAdmin, cancellationToken);
        if (role == null)
        {
            await CreateRoleAsync(RoleCatalog.SuperAdmin, permissions, cancellationToken);
            return;
        }

        var existingPermissionIds = await _dbContext.RolePermissions
            .Where(x => x.RoleId == role.Id)
            .Select(x => x.PermissionId)
            .ToListAsync(cancellationToken);

        var missingRolePermissions = permissions
            .Where(x => !existingPermissionIds.Contains(x.Id))
            .Select(x => RolePermission.Create(role.Id, x.Id))
            .ToList();

        if (missingRolePermissions.Count == 0)
        {
            return;
        }

        await _dbContext.RolePermissions.AddRangeAsync(missingRolePermissions, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task CreateRoleIfMissingAsync(string roleName, IReadOnlyCollection<Permission> permissions, CancellationToken cancellationToken)
    {
        var roleExists = await _dbContext.Roles.AnyAsync(x => x.Name == roleName, cancellationToken);
        if (roleExists)
        {
            return;
        }

        await CreateRoleAsync(roleName, permissions, cancellationToken);
    }

    private async Task CreateRoleAsync(string roleName, IReadOnlyCollection<Permission> permissions, CancellationToken cancellationToken)
    {
        var role = Role.Create(roleName, null);

        await _dbContext.Roles.AddAsync(role, cancellationToken);
        var rolePermissions = permissions
            .Select(x => RolePermission.Create(role.Id, x.Id))
            .ToList();

        await _dbContext.RolePermissions.AddRangeAsync(rolePermissions, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region User and UserRole helper
    public async Task SeedUsersAsync(CancellationToken cancellationToken = default)
    {
        var currentTenantId = _currentTenantService.TenantId;

        if (string.IsNullOrWhiteSpace(currentTenantId))
        {
            throw new InvalidOperationException("Current tenant is not available.");
        }

        if (currentTenantId == MultitenancyConstants.RootTenantId)
        {
            await CreateUserIfMissingAsync(RoleCatalog.SuperAdmin.ToLower(), RoleCatalog.SuperAdmin, cancellationToken);
            return;
        }

        await CreateUserIfMissingAsync(RoleCatalog.Admin.ToLower(), RoleCatalog.Admin, cancellationToken);
        await CreateUserIfMissingAsync(RoleCatalog.User.ToLower(), RoleCatalog.User, cancellationToken);
    }

    private async Task CreateUserIfMissingAsync(string username, string roleName, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == username, cancellationToken);
        var role = await _dbContext.Roles.SingleAsync(x => x.Name == roleName, cancellationToken);

        if (user == null)
        {
            user = User.Create(username, string.Empty);
            var passwordHash = _passwordHasher.HashPassword(user, username);
            user.UpdatePassword(passwordHash);

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var userRoleExists = await _dbContext.UserRoles
            .AnyAsync(x => x.UserId == user.Id && x.RoleId == role.Id, cancellationToken);

        if (userRoleExists)
        {
            return;
        }

        var userRole = UserRole.Assign(role.Id, user.Id);

        await _dbContext.UserRoles.AddAsync(userRole, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion
}