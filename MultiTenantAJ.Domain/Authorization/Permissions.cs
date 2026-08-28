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
            #region PropertyManagement
            
            new(NameFor(ActionCatalog.View, ResourceCatalog.Properties)),
            new(NameFor(ActionCatalog.Search, ResourceCatalog.Properties)),
            new(NameFor(ActionCatalog.Create, ResourceCatalog.Properties)),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.Properties)),
            new(NameFor(ActionCatalog.Delete, ResourceCatalog.Properties)),

            new(NameFor(ActionCatalog.View, ResourceCatalog.Guests)),
            new(NameFor(ActionCatalog.Search, ResourceCatalog.Guests)),
            new(NameFor(ActionCatalog.Create, ResourceCatalog.Guests)),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.Guests)),
            new(NameFor(ActionCatalog.Delete, ResourceCatalog.Guests)),

            new(NameFor(ActionCatalog.View, ResourceCatalog.Reservations)),
            new(NameFor(ActionCatalog.Search, ResourceCatalog.Reservations)),
            new(NameFor(ActionCatalog.Create, ResourceCatalog.Reservations)),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.Reservations)),
            new(NameFor(ActionCatalog.Delete, ResourceCatalog.Reservations)),

            new(NameFor(ActionCatalog.View, ResourceCatalog.MaintenanceRequests)),
            new(NameFor(ActionCatalog.Search, ResourceCatalog.MaintenanceRequests)),
            new(NameFor(ActionCatalog.Create, ResourceCatalog.MaintenanceRequests)),
            new(NameFor(ActionCatalog.Update, ResourceCatalog.MaintenanceRequests)),
            new(NameFor(ActionCatalog.Delete, ResourceCatalog.MaintenanceRequests)),
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

            new(NameFor(ActionCatalog.Search, ResourceCatalog.Permissions)),
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