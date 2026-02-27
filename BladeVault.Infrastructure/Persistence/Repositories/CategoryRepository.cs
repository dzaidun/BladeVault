using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

        public async Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.ParentCategoryId == null && x.IsActive)
                .Include(x => x.SubCategories)
                .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Category>> GetSubCategoriesAsync(Guid parentId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.ParentCategoryId == parentId && x.IsActive)
                .ToListAsync(cancellationToken);

        public async Task<Category?> GetWithSubCategoriesAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.SubCategories)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<bool> IsSlugUniqueAsync(string slug, CancellationToken cancellationToken = default)
            => !await _dbSet.AnyAsync(x => x.Slug == slug, cancellationToken);
    }
}
