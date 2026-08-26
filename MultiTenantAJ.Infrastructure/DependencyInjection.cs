using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using MultiTenantAJ.Infrastructure.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;
using MultiTenantAJ.Infrastructure.Seeder;

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
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Database")));
        services.AddScoped<IdentitySeeder>();
        services.AddScoped<DatabaseInitializer>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(
            typeof(IRepository<>),
            typeof(ApplicationDbRepository<>));

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<TenantResolver>();

        return app;
    }
}
