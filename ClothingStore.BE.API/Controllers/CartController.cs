using System.Threading;
using System.Threading.Tasks;
using ClothingStore.Application.Features.Carts.Commands;
using ClothingStore.Application.Features.Carts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Application.Common.Commands;
using Shared.Application.Abstractions.DTOs;
using Shared.Domain.Common.ResponseModel;
using Shared.Presentation.Common;
using Shared.Presentation.Common.Attributes;
using ClothingStore.Application.Features.Carts.Queries;

namespace ClothingStore.API.Controllers
{
    [Route("api/[controller]")]
    public class CartsController : ApiController
    {
        public CartsController(IMediator mediator) : base(mediator) { }

        [HttpPost("add")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> AddToCart(
            [FromCurrentUser] CurrentUserDto user,
            [FromBody] AddToCartDto item,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddToCartCommand(user.UserId, item), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> UpdateItem(
            [FromCurrentUser] CurrentUserDto user,
            [FromBody] UpdateCartItemDto item,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateCartItemCommand(user.UserId, item), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpPost("remove")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> RemoveItem(
            [FromCurrentUser] CurrentUserDto user,
            [FromBody] RemoveCartItemDto item,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new RemoveCartItemCommand(user.UserId, item), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpPost("checkout")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> Checkout(
            [FromCurrentUser] CurrentUserDto user,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CartCheckoutCommand(user.UserId), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> GetCart(
    [FromCurrentUser] CurrentUserDto user,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCartByUserIdQuery(user.UserId), cancellationToken);
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
