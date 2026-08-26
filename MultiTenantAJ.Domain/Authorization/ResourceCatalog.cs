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
    public const string Users = nameof(Users);
    public const string Roles = nameof(Roles);
    public const string RolePermissions = nameof(RolePermissions);
    public const string UserRoles = nameof(UserRoles);

    #endregion

    #region Tenant
    public const string Tenants = nameof(Tenants);
    #endregion
}
