using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Enums;
using ClothingStore.Domain.Repositories;
using ClothingStore.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Common;

namespace ClothingStore.Infrastructure.Repositories
{
    public class ExternalIdentityRepository 
        : GenericRepository<ExternalIdentity>, IExternalIdentityRepository
    {
        private readonly DbSet<ExternalIdentity> _dbSet;

        public ExternalIdentityRepository(UsersDbContext context) : base(context)
        {
            _dbSet = context.Set<ExternalIdentity>();
        }

        public async Task<ExternalIdentity?> FindExternalAsync(
            AuthProvider provider,
            string providerUserId,
            CancellationToken ct)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    e => e.Provider == provider && e.ProviderUserId == providerUserId,
                    ct);
        }
    }
}
