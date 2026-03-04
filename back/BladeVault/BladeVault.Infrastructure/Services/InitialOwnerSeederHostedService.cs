using BladeVault.Domain.Entities;
using BladeVault.Domain.Enums;
using BladeVault.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BladeVault.Infrastructure.Services
{
    public class InitialOwnerSeederHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<InitialOwnerSeederHostedService> _logger;

        public InitialOwnerSeederHostedService(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration,
            ILogger<InitialOwnerSeederHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var settings = _configuration
                .GetSection(InitialOwnerSettings.SectionName)
                .Get<InitialOwnerSettings>();

            if (settings is null ||
                string.IsNullOrWhiteSpace(settings.Email) ||
                string.IsNullOrWhiteSpace(settings.Password) ||
                string.IsNullOrWhiteSpace(settings.FirstName) ||
                string.IsNullOrWhiteSpace(settings.LastName) ||
                string.IsNullOrWhiteSpace(settings.PhoneNumber))
            {
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            var existingUser = await uow.Users.GetByEmailAsync(settings.Email, cancellationToken);
            if (existingUser is not null)
            {
                return;
            }

            var owner = User.Create(
                firstName: settings.FirstName,
                lastName: settings.LastName,
                email: settings.Email,
                phoneNumber: settings.PhoneNumber,
                passwordHash: passwordHasher.Hash(settings.Password),
                role: UserRole.Owner);

            await uow.Users.AddAsync(owner, cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Initial Owner account has been created for {Email}", settings.Email);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
