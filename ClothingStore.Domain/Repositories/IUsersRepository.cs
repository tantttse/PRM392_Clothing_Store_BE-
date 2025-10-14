using ClothingStore.Domain.Entities;
using Shared.Application.Abstractions.Repositories;

namespace ClothingStore.Domain.Repositories
{
    public interface IUserRepository : IRepository<Users>
    {
        Task<Users?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<Users?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Users?> LoginAsync(string userNameOrEmail, string password, CancellationToken cancellationToken = default);
        Task<Users?> GetUserByMailOrUserName(string userNameOrEmail, CancellationToken cancellationToken = default);

    }
}
