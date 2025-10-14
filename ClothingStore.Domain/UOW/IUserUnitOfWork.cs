using ClothingStore.Domain.Repositories;
using Shared.Application.Abstractions.UnitOfWork;

namespace ClothingStore.Application.Abstractions.UnitOfWork
{
    public interface IUserUnitOfWork : IUnitOfWork
    {
        // You can add user-specific repository shortcuts here if needed
        IUserRepository Users { get; }
    }

}