using System.Threading;
using System.Threading.Tasks;
using ClothingStore.Application.Features.Products.Commands;
using ClothingStore.Application.Features.Products.Dtos;
using ClothingStore.Application.Features.Products.Queries;
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
    public class ProductsController : ApiController
    {
        public ProductsController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> Create(
            [FromCurrentUser] CurrentUserDto user,
            [FromBody] ProductCreateCommand command,
            CancellationToken cancellationToken)
        {
            // Optionally attach user info to command if needed
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromCurrentUser] CurrentUserDto user,
            [FromBody] ProductUpdateCommand command,
            CancellationToken cancellationToken)
        {
            command = command with { Id = id };

            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> Delete(
            Guid id,
            [FromCurrentUser] CurrentUserDto user,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ProductDeleteCommand(id), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            return Ok(result);
        }

        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetList([FromQuery] ProductFilterDto filter, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductListQuery(filter), cancellationToken);
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
