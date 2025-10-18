using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using ClothingStore.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Common;

namespace ClothingStore.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly UsersDbContext _context;
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(UsersDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Order>();
        }

        public async Task<List<Order>?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
