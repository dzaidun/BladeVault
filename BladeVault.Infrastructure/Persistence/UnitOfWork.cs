using BladeVault.Domain.Interfaces;
using BladeVault.Domain.Interfaces.Repositories;
using BladeVault.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BladeVault.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        // Репозиторії — ледаче створення (lazy)
        private IKnifeRepository? _knives;
        private IMultiToolRepository? _multiTools;
        private IProductRepository? _products;
        private ICategoryRepository? _categories;
        private IStockRepository? _stock;
        private IStockMovementRepository? _stockMovements;
        private IOrderRepository? _orders;
        private IUserRepository? _users;
        private ICallLogRepository? _callLogs;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Якщо репозиторій ще не створений — створюємо, інакше повертаємо існуючий
        public IKnifeRepository Knives
            => _knives ??= new KnifeRepository(_context);

        public IMultiToolRepository MultiTools
            => _multiTools ??= new MultiToolRepository(_context);

        public IProductRepository Products
            => _products ??= new ProductRepository(_context);

        public ICategoryRepository Categories
            => _categories ??= new CategoryRepository(_context);

        public IStockRepository Stock
            => _stock ??= new StockRepository(_context);

        public IStockMovementRepository StockMovements
            => _stockMovements ??= new StockMovementRepository(_context);

        public IOrderRepository Orders
            => _orders ??= new OrderRepository(_context);

        public IUserRepository Users
            => _users ??= new UserRepository(_context);

        public ICallLogRepository CallLogs
            => _callLogs ??= new CallLogRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
            => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("Транзакція не розпочата");

            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("Транзакція не розпочата");

            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
