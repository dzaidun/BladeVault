using BladeVault.Domain.Interfaces;

namespace BladeVault.Infrastructure.Services
{
    public class MockShipmentTrackingProvider : IShipmentTrackingProvider
    {
        public Task<string> GenerateTrackingNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            var suffix = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            return Task.FromResult($"NP-MOCK-{orderNumber}-{suffix}");
        }
    }
}
