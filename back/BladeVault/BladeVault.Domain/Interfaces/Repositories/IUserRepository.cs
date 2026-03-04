using BladeVault.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
        Task<User?> GetWithAddressesAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken = default);
    }
}
