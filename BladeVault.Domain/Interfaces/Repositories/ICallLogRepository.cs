using BladeVault.Domain.Entities;

namespace BladeVault.Domain.Interfaces.Repositories
{
    public interface ICallLogRepository : IRepository<CallLog>
    {
        Task<IReadOnlyList<CallLog>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CallLog>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}
