using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MultiTenantAJ.Application.Identity;
using MultiTenantAJ.Application.Identity.Users;
using MultiTenantAJ.Application.Multitenancy;
using MultiTenantAJ.Domain.Models.Identity;
using MultiTenantAJ.Domain.Repositories;
using MultiTenantAJ.Infrastructure.Identity;
using MultiTenantAJ.Infrastructure.Multitenancy;
using MultiTenantAJ.Infrastructure.Persistence;
using MultiTenantAJ.Infrastructure.Seeder;
using System.Text;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Multitenancy

        services.AddDbContext<TenantDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));

        services.AddScoped<CurrentTenantService>();

        services.AddScoped<ICurrentTenantService>(provider =>
            provider.GetRequiredService<CurrentTenantService>());

        services.AddScoped<TenantService>();

        #endregion

        #region Identity

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, TokenService>();

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),

                    ValidateLifetime = true
                };
            });

        services.AddAuthorization();

        #endregion

        #region Persistence

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>),typeof(ApplicationDbRepository<>));

        #endregion

        #region Seeder

        services.AddScoped<IdentitySeeder>();
        services.AddScoped<DatabaseInitializer>();

        #endregion

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<TenantResolver>();
        return app;
    }
}