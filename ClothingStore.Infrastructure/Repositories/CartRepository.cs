using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using ClothingStore.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Common;

namespace ClothingStore.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly UsersDbContext _context;
        private readonly DbSet<Cart> _dbSet;

        public CartRepository(UsersDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Cart>();
        }

        public async Task<Cart?> GetActiveCartByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await GetQueryable(asNoTracking: false) 
                        .Include(c => c.Items) // eager load items if you’ll modify them
                        .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active", cancellationToken);
        }

    }
}
