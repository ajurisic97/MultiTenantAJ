# MultiTenantAJ

MultiTenantAJ is a multi-tenant ASP.NET Core Web API with tenant data isolation, hybrid database support, JWT authentication and permission-based authorization.

## Setup

### Requirements

- .NET 10 SDK
- PostgreSQL

### Database Configuration

Before running the application, configure the PostgreSQL connection string under `ConnectionStrings:Database`.

```json
{
  "ConnectionStrings": {
    "Database": "Host=localhost;Port=5432;Database=rootTenantDb;Username=postgres;Password=your_password"
  }
}
```

Replace the connection values with those for your local PostgreSQL installation.

The application automatically applies required database migrations and initializes tenant and identity data on startup.

### Demo Data

Demo business data is seeded automatically by default.

To disable demo data, set `SeedDemoData` to `false` in `appsettings.json`:

```json
{
  "SeedDemoData": false
}
```

Identity data, roles, permissions and default users are initialized regardless of this setting.

## Default Users

For every non-root tenant, two users are created automatically:

- `admin` – assigned to the `Admin` role with all permissions available to that tenant
- `user` – assigned to the `User` role without permissions by default

The initial password is the same as the username:

```text
admin / admin
user / user
```

The root tenant is initialized separately with:

```text
superadmin / superadmin
```

The `SuperAdmin` role has access to system-level tenant management permissions.

If the Maintenance module is disabled for a tenant, Maintenance permissions are not available to its Admin role.

> Default credentials are intended for development and testing only.

## Main Features

- tenant identification using API keys and JWT tenant claims
- tenant-specific data isolation using `TenantId`
- Entity Framework Core global query filters
- protection against cross-tenant data modifications
- shared database support
- separate database support through tenant-specific connection strings
- hybrid multi-tenant database configuration
- automatic database migrations and tenant initialization
- JWT authentication
- role and permission management
- root-only tenant administration
- tenant activation and deactivation
- tenant-specific feature configuration
- Maintenance Requests module that can be enabled or disabled per tenant
- automatic removal of Maintenance role-permission relationships when the module is disabled
- Swagger / OpenAPI support

## Tenant Identification

Before authentication, the tenant is identified through the `tenant` HTTP header using the tenant API key.

After authentication, the tenant is determined from the tenant claim stored in the JWT.

## Tenant-Specific Maintenance Module

Each tenant has a `MaintenanceEnabled` setting.

When the module is disabled:

- Maintenance endpoints are blocked
- Maintenance permissions are hidden
- Maintenance permissions cannot be assigned to roles
- existing Maintenance role-permission relationships for that tenant are removed

When the module is enabled again, Maintenance permissions become available and can be assigned through the existing role management functionality.

## Solution Structure

- `MultiTenantAJ.Api` – API controllers, HTTP contracts and middleware configuration
- `MultiTenantAJ.Application` – CQRS commands and queries, validation and application logic
- `MultiTenantAJ.Domain` – domain models, authorization definitions and domain rules
- `MultiTenantAJ.Infrastructure` – Entity Framework Core, PostgreSQL persistence, authentication, tenant resolution, database initialization and seeding