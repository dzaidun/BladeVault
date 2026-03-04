using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Enums.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Interfaces.Repositories
{
    public interface IKnifeRepository : IRepository<Knife>
    {
        Task<Knife?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Knife>> GetByFiltersAsync(
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
            CancellationToken cancellationToken = default);
    }
}
