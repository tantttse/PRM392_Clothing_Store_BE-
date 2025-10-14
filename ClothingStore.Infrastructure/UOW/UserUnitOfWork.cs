using ClothingStore.Application.Abstractions.UnitOfWork;
using ClothingStore.Domain.Repositories;
using ClothingStore.Infrastructure.Persistence;
using ClothingStore.Infrastructure.Persistence.Contexts;
using Shared.Infrastructure.Common;

namespace ClothingStore.Infrastructure.Common
{
    public class UserUnitOfWork 
        : GenericUnitOfWork<UsersDbContext>, IUserUnitOfWork
    {
        private readonly IUserRepository _userRepository;

        public UserUnitOfWork(UsersDbContext dbContext, IUserRepository userRepository)
            : base(dbContext)
        {
            _userRepository = userRepository;
        }

        public IUserRepository Users => _userRepository;
    }

}
