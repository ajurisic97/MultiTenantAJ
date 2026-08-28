using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Authorization;

public static class ResourceCatalog
{
    #region Catalog
    public const string Products = nameof(Products);
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
