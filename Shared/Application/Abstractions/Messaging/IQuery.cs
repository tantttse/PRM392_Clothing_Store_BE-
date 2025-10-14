using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Domain.Common.ResponseModel;
using MediatR;

namespace Shared.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }

}
