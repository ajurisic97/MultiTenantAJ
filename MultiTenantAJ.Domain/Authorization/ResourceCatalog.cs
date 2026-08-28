using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Authorization;

public static class ResourceCatalog
{
    #region PropertyManagement
    public const string Properties = nameof(Properties);
    public const string Guests = nameof(Guests);
    public const string Reservations = nameof(Reservations);
    public const string MaintenanceRequests = nameof(MaintenanceRequests);

    #endregion

    #region Identity
    public const string Users = nameof(Users); //pripadajuce UserRoles isto spadaju tu
    public const string Roles = nameof(Roles); //pripadajuce RolePermissions isto spadaju tu
    public const string Permissions = nameof(Permissions);


    #endregion

    #region Tenant
    public const string Tenants = nameof(Tenants);
    #endregion
}
