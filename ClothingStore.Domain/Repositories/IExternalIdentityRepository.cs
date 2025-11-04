using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Enums;
using Shared.Application.Abstractions.Repositories;

namespace ClothingStore.Domain.Repositories
{
    public interface IExternalIdentityRepository : IRepository<ExternalIdentity>
    {
        Task<ExternalIdentity?> FindExternalAsync(AuthProvider provider, string providerUserId, CancellationToken ct);
    }
}
