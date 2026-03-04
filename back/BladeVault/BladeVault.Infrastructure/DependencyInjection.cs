using BladeVault.Domain.Interfaces;
using BladeVault.Infrastructure.Persistence;
using BladeVault.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BladeVault.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // БД
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IShipmentTrackingProvider, MockShipmentTrackingProvider>();

            // Seed initial owner
            services.AddHostedService<InitialOwnerSeederHostedService>();

            // JwtSettings
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            return services;
        }
    }
}
