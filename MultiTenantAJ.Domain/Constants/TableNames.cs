using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Constants;

public static class TableNames
{
    public const string Tenants = nameof(Tenants);

    #region Identity
    public const string Users = nameof(Users);
    public const string Roles = nameof(Roles);
    public const string Permissions = nameof(Permissions);
    public const string UserRoles = nameof(UserRoles);
    public const string RolePermissions = nameof(RolePermissions);
    #endregion

    #region PropertyManagement

    public const string Properties = nameof(Properties);
    public const string Guests = nameof(Guests);
    public const string Reservations = nameof(Reservations);
    public const string MaintenanceRequests = nameof(MaintenanceRequests);

    #endregion
}
