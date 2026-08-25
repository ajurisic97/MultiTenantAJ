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
        services.AddScoped<TenantService>();


        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Database")));

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<TenantResolver>();

        return app;
    }
}
