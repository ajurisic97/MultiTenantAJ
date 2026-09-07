# MultiTenantAJ

MultiTenantAJ is an ASP.NET Core Web API developed as the practical part of a master's thesis focused on the implementation of multi-tenant architecture and tenant data isolation in business applications.

The project demonstrates tenant identification, data isolation, hybrid database usage, authentication and authorization, tenant lifecycle management, and tenant-specific feature configuration.

## Database Initialization and Seeding

The application automatically performs the required database initialization when it starts.

Identity data is always initialized for each tenant and password is equal to username.

For every non-root tenant, two default users are created:

- `admin` – assigned to the Admin role. 
- `user` – assigned to the User role without permissions by default

The Admin role receives the permissions available to the tenant. Root-only permissions are excluded, and Maintenance permissions are also excluded when the tenant has `MaintenanceEnabled` set to `false`.

The root tenant is initialized separately with the `SuperAdmin` role and the permissions required for system-level administration.

The application can also automatically seed demo business data for testing and demonstration purposes.

Demo data seeding can be disabled through application configuration. If demo data is not required, set the demo data flag `SeedDemoData` to `false` in appsettings.json.

## Solution Architecture

The solution is divided into the following projects:

- `MultiTenantAJ.Api` – API endpoints, middleware pipeline and HTTP contracts
- `MultiTenantAJ.Application` – application logic, CQRS commands and queries, validation and application services
- `MultiTenantAJ.Domain` – domain models, authorization definitions and domain rules
- `MultiTenantAJ.Infrastructure` – Entity Framework Core, authentication, tenant resolution, database initialization and persistence

## Multi-Tenancy

The application supports multiple tenants while using the same application codebase.

Tenant identification depends on the current request:

- unauthenticated requests use the `tenant` HTTP header containing the tenant API key
- authenticated requests use the tenant claim stored in the JWT

After the tenant has been resolved, its information is stored in the current tenant context and used throughout the request.

Tenant-owned entities contain a `TenantId`. Entity Framework Core global query filters automatically restrict queries to records belonging to the current tenant.

The application also validates added, modified and deleted entities before saving changes in order to prevent cross-tenant data modifications.

## Database Models

The application supports a hybrid multi-tenant database model.

Multiple tenants can share the default database while their records are isolated using `TenantId`.

A tenant can also specify another connection string, allowing tenant data to be stored in a different database.

This makes it possible to demonstrate both:

- shared database usage with tenant-level data isolation
- separate database usage for selected tenants

The central tenant registry is accessed through `TenantDbContext`, while business and identity data are accessed through `ApplicationDbContext`.

## Authentication and Authorization

The application uses JWT authentication.

JWT tokens contain information about:

- the authenticated user
- the current tenant
- user roles
- user permissions

Authorization is based on permission policies.

Individual API operations require the corresponding permission, while root-only permissions are restricted to the root tenant.

The root tenant is used for system-level tenant management.

## Tenant Management

The root tenant can manage application tenants through the API.

Supported operations include:

- creating a tenant
- retrieving tenants
- activating or deactivating a tenant
- enabling or disabling tenant-specific functionality

When a new tenant is created, the application initializes the required database structure and identity data before activating the tenant.

## Tenant-Specific Configuration

The project demonstrates tenant-specific application configuration through the `MaintenanceEnabled` setting.

This setting determines whether the Maintenance Requests module is available to a tenant.

When Maintenance is disabled for a tenant:

- Maintenance API endpoints are blocked
- Maintenance permissions are not returned as available permissions
- Maintenance permissions cannot be assigned to roles
- existing Maintenance role-permission relationships for that tenant are removed

When Maintenance is enabled again, the permissions become available and can be assigned to roles through the existing role and permission management functionality.

Feature availability and user authorization are handled separately.

A user can access a Maintenance operation only when:

1. the Maintenance module is enabled for the tenant
2. the user has the permission required by the requested operation

