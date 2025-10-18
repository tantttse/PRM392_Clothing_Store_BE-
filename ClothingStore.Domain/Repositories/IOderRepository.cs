using ClothingStore.Domain.Entities;
using Shared.Application.Abstractions.Repositories;

namespace ClothingStore.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        // Task<Users?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        // Task<Users?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        // Task<Users?> LoginAsync(string userNameOrEmail, string password, CancellationToken cancellationToken = default);
        // Task<Users?> GetUserByMailOrUserName(string userNameOrEmail, CancellationToken cancellationToken = default);
        Task<List<Order>?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
