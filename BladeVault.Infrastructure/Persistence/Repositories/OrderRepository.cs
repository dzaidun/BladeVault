using BladeVault.Domain.Entities;
using BladeVault.Domain.Enums;
using BladeVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);

        public async Task<Order?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<Order?> GetFullOrderAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.User)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<IReadOnlyList<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Items)
                .Include(x => x.Payment)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.User)
                .Include(x => x.Items)
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Order>> GetForWarehouseAsync(CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .Where(x => x.Status == OrderStatus.Confirmed || x.Status == OrderStatus.InAssembly)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Order>> GetForAnalyticsAsync(
            DateTime from,
            DateTime to,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Items)
                .Where(x => x.CreatedAt >= from && x.CreatedAt <= to)
                .ToListAsync(cancellationToken);
    }
}
