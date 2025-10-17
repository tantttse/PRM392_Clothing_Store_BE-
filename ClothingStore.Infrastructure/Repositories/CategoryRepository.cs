using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using ClothingStore.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Common;

namespace ClothingStore.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly UsersDbContext _context;
        private readonly DbSet<Product> _dbSet;

        public CategoryRepository(UsersDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Product>();
        }

    }
}
