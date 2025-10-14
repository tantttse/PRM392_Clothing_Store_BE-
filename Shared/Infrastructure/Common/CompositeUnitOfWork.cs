using Shared.Application.Abstractions.UnitOfWork;

namespace Shared.Infrastructure.Common
{
    public class CompositeUnitOfWork : ICompositeUnitOfWork
    {
        private readonly IEnumerable<ISaveChangesUnitOfWork> _units;

        public CompositeUnitOfWork(IEnumerable<ISaveChangesUnitOfWork> units)
        {
            _units = units; // No need to filter CompositeUnitOfWork since it’s no longer ISaveChangesUnitOfWork
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var total = 0;
            foreach (var uow in _units)
            {
                total += await uow.SaveChangesAsync(cancellationToken);
            }
            return total;
        }
    }
}