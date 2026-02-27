using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Enums.ProductSpecs;
using BladeVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence.Repositories
{
    public class MultiToolRepository : Repository<MultiTool>, IMultiToolRepository
    {
        public MultiToolRepository(AppDbContext context) : base(context) { }

        public async Task<MultiTool?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
            => await _context.MultiTools
                .Include(x => x.IncludedTools)
                .Include(x => x.Images)
                .Include(x => x.Stock)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

        public async Task<MultiTool?> GetWithToolsAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.MultiTools
                .Include(x => x.IncludedTools)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<IReadOnlyList<MultiTool>> GetByFiltersAsync(
            bool? hasPliers = null,
            bool? replaceableWireCutters = null,
            bool? hasLocking = null,
            bool? isOneHandOpenable = null,
            ToolType? includedTool = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.MultiTools
                .Include(x => x.IncludedTools)
                .Include(x => x.Images)
                .Include(x => x.Stock)
                .Where(x => x.IsActive)
                .AsQueryable();

            if (hasPliers.HasValue)
                query = query.Where(x => x.HasPliers == hasPliers.Value);

            if (replaceableWireCutters.HasValue)
                query = query.Where(x => x.ReplaceableWireCutters == replaceableWireCutters.Value);

            if (hasLocking.HasValue)
                query = query.Where(x => x.HasLocking == hasLocking.Value);

            if (isOneHandOpenable.HasValue)
                query = query.Where(x => x.IsOneHandOpenable == isOneHandOpenable.Value);

            if (includedTool.HasValue)
                query = query.Where(x => x.IncludedTools.Any(t => t.Type == includedTool.Value));

            if (minPrice.HasValue)
                query = query.Where(x => x.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(x => x.Price <= maxPrice.Value);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
