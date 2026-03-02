using BladeVault.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IKnifeRepository Knives { get; }
        IMultiToolRepository MultiTools { get; }
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IStockRepository Stock { get; }
        IStockMovementRepository StockMovements { get; }
        IOrderRepository Orders { get; }
        IUserRepository Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
