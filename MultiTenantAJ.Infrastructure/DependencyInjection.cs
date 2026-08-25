using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Infrastructure.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenantAJ.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TenantDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Database")));

        services.AddScoped<CurrentTenantService>();

        services.AddScoped<ICurrentTenantService>(provider =>
            provider.GetRequiredService<CurrentTenantService>());

        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var currentTenant =
                provider.GetRequiredService<ICurrentTenantService>();

            if (string.IsNullOrWhiteSpace(currentTenant.ConnectionString))
            {
                throw new InvalidOperationException(
                    "Tenant connection string is not available.");
            }

            options.UseNpgsql(currentTenant.ConnectionString);
        });

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<TenantResolver>();

        return app;
    }
}
