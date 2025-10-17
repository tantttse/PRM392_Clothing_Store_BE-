using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using ClothingStore.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Common;

namespace ClothingStore.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly UsersDbContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductRepository(UsersDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Product>();
        }

    }
}
