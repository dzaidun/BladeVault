using BladeVault.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetSubCategoriesAsync(Guid parentId, CancellationToken cancellationToken = default);
        Task<Category?> GetWithSubCategoriesAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsSlugUniqueAsync(string slug, CancellationToken cancellationToken = default);
    }
}
