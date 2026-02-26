using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Enums.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Interfaces.Repositories
{
    public interface IMultiToolRepository : IRepository<MultiTool>
    {
        Task<MultiTool?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<MultiTool?> GetWithToolsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MultiTool>> GetByFiltersAsync(
            bool? hasPliers = null,
            bool? replaceableWireCutters = null,
            bool? hasLocking = null,
            bool? isOneHandOpenable = null,
            ToolType? includedTool = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            CancellationToken cancellationToken = default);
    }
}
