using E_Commerce.Application.Services.Contracts;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Contracts.Repositories;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.DataSeeding;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Services;
using E_Commerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace E_Commerce.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreDbContext>(Options => {
            Options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

        services.AddDbContext<StoreIdentityDbContext>(Options => {
            Options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

        services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");
        services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IConnectionMultiplexer>(config =>
        {
            return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConncetion"));
        });

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ICacheRepository, CacheRepository>();

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<StoreIdentityDbContext>();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = "https://localhost:7116",
                ValidateAudience = true,
                ValidAudience = "MyOnlineStore",
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MYSECRETKEYforMyApplicationMYSECRETKEYforMyApplicationMYSECRETKEYforMyApplicationMYSECRETKEYforMyApplication"))
            };
        });



        return services;
    }
}
