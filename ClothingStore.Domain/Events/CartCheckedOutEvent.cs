using Shared.Domain.Common.DDD;

namespace ClothingStore.Domain.Events;

public record CartCheckedOutEvent(
    Guid CartId,
    Guid UserId,
    decimal TotalPrice,
    DateTime CheckedOutAt
) : DomainEvent;
