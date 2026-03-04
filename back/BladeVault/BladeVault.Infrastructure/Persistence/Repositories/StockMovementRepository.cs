using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence.Repositories
{
    public class StockMovementRepository : Repository<StockMovement>, IStockMovementRepository
    {
        public StockMovementRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<StockMovement>> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.ProductId == productId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
    }
}
