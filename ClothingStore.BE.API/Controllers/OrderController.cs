using System.Threading;
using System.Threading.Tasks;
using ClothingStore.Application.Features.Orders.Commands;
using ClothingStore.Application.Features.Orders.Dtos;
using ClothingStore.Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Common.Commands;
using Shared.Application.Abstractions.DTOs;
using Shared.Domain.Common.ResponseModel;
using Shared.Presentation.Common;
using Shared.Presentation.Common.Attributes;

namespace ClothingStore.API.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : ApiController
    {
        public OrdersController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> Create(
            [FromCurrentUser] CurrentUserDto user,
            [FromBody] CreateOrderCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { UserId = user.UserId };

            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpPut("status")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> UpdateStatus(
            [FromBody] UpdateOrderStatusCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpPut("cancel/{orderId:guid}")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> Cancel(
            Guid orderId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CancelOrderCommand(orderId), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> GetMyOrders(
            [FromCurrentUser] CurrentUserDto user,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOrdersByUserQuery(user.UserId), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            return Ok(result);
        }

        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy" });
        }
    }
}
