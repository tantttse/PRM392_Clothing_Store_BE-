using MediatR;
using Shared.Domain.Common.ResponseModel;

namespace Shared.Application.Common.Commands
{
    public sealed record SaveChangesCommand() : IRequest<Result>;
}


