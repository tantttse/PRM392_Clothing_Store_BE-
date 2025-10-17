
// using ClothingStore.Application.Features.User.Commands.RefreshToken;
using ClothingStore.Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Shared.Presentation.Common;
using Shared.Application.Common.Commands;
using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Application.Features.Products.Commands;
using ClothingStore.Application.Features.Products.Queries;

namespace ClothingStore.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ApiController
    {
        public ProductController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("create")]
        // [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ProductCreateCommand(request), cancellationToken);
            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure)
            {
                return HandleFailure(commit);
            }

            return Ok(result);
        }

        [HttpPost("update/{id:guid}")]
        // [AllowAnonymous]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] ProductUpdateDto request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ProductUpdateCommand(id,request), cancellationToken);
            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure)
            {
                return HandleFailure(commit);
            }

            return Ok(result);
        }

        // [HttpPost("refresh-token")]
        // // [AllowAnonymous]
        // public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
        // {
        //     var result = await _mediator.Send(command, cancellationToken);
        //     if (result.IsFailure)
        //     {
        //         return HandleFailure(result);
        //     }

        //     var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
        //     if (commit.IsFailure)
        //     {
        //         return HandleFailure(commit);
        //     }

        //     return Ok(result);
        // }

        [HttpGet("{id:guid}")]
        // [Authorize("Admin", "User")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result);
        }

        [HttpGet("all")]
        // [Authorize("Admin")]
        public async Task<IActionResult> GetAll([FromQuery] ProductFilterDto request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductListQuery(request), cancellationToken);
            return Ok(result);
        }

    }
}
