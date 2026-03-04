using BladeVault.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Interfaces.Repositories
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Stock>> GetLowStockAsync(int threshold, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Stock>> GetOutOfStockAsync(CancellationToken cancellationToken = default);
    }
}
