using Shared.Domain.Common.DDD;
using ClothingStore.Domain.Enums;

namespace ClothingStore.Domain.Entities;

public class ExternalIdentity : Entity<Guid>
{
    public Guid UserId { get; private set; } = default!;
    public AuthProvider Provider { get; private set; }   // enum 
    public string ProviderUserId { get; private set; } = string.Empty;// Google "sub"
    public virtual Users User { get; private set; } = default!;
    // EF Core constructor
    private ExternalIdentity() { }

    internal ExternalIdentity(Guid userId, AuthProvider provider, string providerUserId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Provider = provider;
        ProviderUserId = providerUserId;
    }
}
