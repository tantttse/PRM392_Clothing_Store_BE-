using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using ClothingStore.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Common;

namespace ClothingStore.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<Users>, IUserRepository
    {
        private readonly UsersDbContext _context;
        private readonly DbSet<Users> _dbSet;

        public UserRepository(UsersDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Users>();
        }

        public async Task<Users?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }

        public async Task<Users?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<Users?> LoginAsync(string userNameOrEmail, string password, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(u => (u.UserName == userNameOrEmail || u.Email == userNameOrEmail) && u.PasswordHash == password, cancellationToken);
        }

        public async Task<Users?> GetUserByMailOrUserName(string userNameOrEmail, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.Email == userNameOrEmail || u.UserName == userNameOrEmail)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
