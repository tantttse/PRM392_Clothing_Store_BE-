using System.Threading;
using System.Threading.Tasks;

namespace Shared.Application.Abstractions.UnitOfWork
{
    public interface ISaveChangesUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}


