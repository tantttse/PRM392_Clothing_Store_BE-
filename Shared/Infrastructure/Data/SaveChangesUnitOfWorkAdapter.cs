using Shared.Application.Abstractions.UnitOfWork;

namespace Shared.Application.Abstractions.Adapters
{
    public class SaveChangesUnitOfWorkAdapter : ISaveChangesUnitOfWork
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaveChangesUnitOfWorkAdapter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
