using BladeVault.Domain.Entities;
using BladeVault.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<Order?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Order?> GetFullOrderAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> GetForWarehouseAsync(CancellationToken cancellationToken = default);
    }
}
