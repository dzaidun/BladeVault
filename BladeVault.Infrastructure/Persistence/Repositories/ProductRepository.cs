using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.SKU == sku, cancellationToken);

        public async Task<bool> IsSlugUniqueAsync(string slug, CancellationToken cancellationToken = default)
            => !await _dbSet.AnyAsync(x => x.Slug == slug, cancellationToken);

        public async Task<bool> IsSkuUniqueAsync(string sku, CancellationToken cancellationToken = default)
            => !await _dbSet.AnyAsync(x => x.SKU == sku, cancellationToken);

        public async Task<IReadOnlyList<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.CategoryId == categoryId && x.IsActive)
                .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.IsActive)
                .ToListAsync(cancellationToken);
    }
}
