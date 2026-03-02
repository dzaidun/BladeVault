using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence.Repositories
{
    public class CallLogRepository : Repository<CallLog>, ICallLogRepository
    {
        public CallLogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<CallLog>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<CallLog>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.OrderId == orderId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
    }
}
