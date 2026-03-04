using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(AppDbContext context) : base(context) { }

        public async Task<Stock?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);

        public async Task<IReadOnlyList<Stock>> GetLowStockAsync(int threshold, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.AvailableQuantity <= threshold && x.AvailableQuantity > 0)
                .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Stock>> GetOutOfStockAsync(CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.AvailableQuantity == 0)
                .ToListAsync(cancellationToken);
    }
}
