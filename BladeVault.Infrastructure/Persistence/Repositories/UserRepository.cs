using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BladeVault.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        public async Task<User?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.PhoneNumber == phone, cancellationToken);

        public async Task<User?> GetWithAddressesAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
            => !await _dbSet.AnyAsync(x => x.Email == email, cancellationToken);

        public async Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken = default)
            => !await _dbSet.AnyAsync(x => x.PhoneNumber == phone, cancellationToken);
    }
}
