using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Enums.ProductSpecs;
using BladeVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence.Repositories
{
    public class KnifeRepository : Repository<Knife>, IKnifeRepository
    {
        public KnifeRepository(AppDbContext context) : base(context) { }

        public async Task<Knife?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
            => await _context.Knives
                .Include(x => x.Images)
                .Include(x => x.Stock)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

        public async Task<IReadOnlyList<Knife>> GetByFiltersAsync(
            KnifeType? knifeType = null,
            string? steelType = null,
            BladeShape? bladeShape = null,
            EdgeType? edgeType = null,
            LockType? lockType = null,
            OpeningMechanism? openingMechanism = null,
            bool? hasClip = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            double? maxBladeLengthMm = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Knives
                .Include(x => x.Images)
                .Include(x => x.Stock)
                .Where(x => x.IsActive)
                .AsQueryable();

            if (knifeType.HasValue)
                query = query.Where(x => x.KnifeType == knifeType.Value);

            if (!string.IsNullOrEmpty(steelType))
                query = query.Where(x => x.SteelType == steelType);

            if (bladeShape.HasValue)
                query = query.Where(x => x.BladeShape == bladeShape.Value);

            if (edgeType.HasValue)
                query = query.Where(x => x.EdgeType == edgeType.Value);

            if (lockType.HasValue)
                query = query.Where(x => x.LockType == lockType.Value);

            if (openingMechanism.HasValue)
                query = query.Where(x => x.OpeningMechanism == openingMechanism.Value);

            if (hasClip.HasValue)
                query = query.Where(x => x.HasClip == hasClip.Value);

            if (minPrice.HasValue)
                query = query.Where(x => x.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(x => x.Price <= maxPrice.Value);

            if (maxBladeLengthMm.HasValue)
                query = query.Where(x => x.BladeLengthMm <= maxBladeLengthMm.Value);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
