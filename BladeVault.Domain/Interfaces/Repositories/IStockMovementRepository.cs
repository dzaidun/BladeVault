using BladeVault.Domain.Entities;

namespace BladeVault.Domain.Interfaces.Repositories
{
    public interface IStockMovementRepository : IRepository<StockMovement>
    {
        Task<IReadOnlyList<StockMovement>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    }
}
