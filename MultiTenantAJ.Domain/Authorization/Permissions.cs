using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Domain.Authorization;

public static class Permissions
{
    public const string ClaimType = "permission";
    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }

    public static IReadOnlyCollection<PermissionDefinition> All { get; } =
        [
            #region Catalog
            
            new(NameFor(ActionCatalog.View, ResourceCatalog.Products)),
            new(NameFor(ActionCatalog.Search, ResourceCatalog.Products)),
            new(NameFor(ActionCatalog.Create, ResourceCatalog.Products)),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.Products)),
            new(NameFor(ActionCatalog.Delete, ResourceCatalog.Products)),

            #endregion

            #region Identity

            new(NameFor(ActionCatalog.View, ResourceCatalog.Users)),
            new(NameFor(ActionCatalog.Search, ResourceCatalog.Users)),
            new(NameFor(ActionCatalog.Create, ResourceCatalog.Users)),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.Users)),
            new(NameFor(ActionCatalog.Delete, ResourceCatalog.Users)),

            new(NameFor(ActionCatalog.View, ResourceCatalog.Roles)),
            new(NameFor(ActionCatalog.Search, ResourceCatalog.Roles)),
            new(NameFor(ActionCatalog.Create, ResourceCatalog.Roles)),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.Roles)),
            new(NameFor(ActionCatalog.Delete, ResourceCatalog.Roles)),

            new(NameFor(ActionCatalog.View, ResourceCatalog.RolePermissions)),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.RolePermissions)),

            new(NameFor(ActionCatalog.View, ResourceCatalog.UserRoles)),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.UserRoles)),

            #endregion

            #region Tenant

            new(NameFor(ActionCatalog.View, ResourceCatalog.Tenants), true),
            new(NameFor(ActionCatalog.Search, ResourceCatalog.Tenants), true),
            new(NameFor(ActionCatalog.Create, ResourceCatalog.Tenants), true),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.Tenants), true),
            new(NameFor(ActionCatalog.Delete, ResourceCatalog.Tenants), true),

            #endregion
        ];
}