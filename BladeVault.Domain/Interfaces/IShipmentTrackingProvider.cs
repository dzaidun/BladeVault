namespace BladeVault.Domain.Interfaces
{
    public interface IShipmentTrackingProvider
    {
        Task<string> GenerateTrackingNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    }
}
