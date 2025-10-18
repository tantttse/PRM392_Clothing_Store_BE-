using System.Threading;
using System.Threading.Tasks;
using ClothingStore.Application.Features.Categories.Commands;
using ClothingStore.Application.Features.Categories.Dtos;
using ClothingStore.Application.Features.Categories.Queries;
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
    public class CategoriesController : ApiController
    {
        public CategoriesController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> Create(
            [FromCurrentUser] CurrentUserDto user,
            [FromBody] CreateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Guest,Customer")]
        public async Task<IActionResult> Update(
            [FromCurrentUser] CurrentUserDto user,
            [FromBody] UpdateCategoryCommand command,
            CancellationToken cancellationToken)
        {
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
            var result = await _mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            var commit = await _mediator.Send(new SaveChangesCommand(), cancellationToken);
            if (commit.IsFailure) return HandleFailure(commit);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
            if (result.IsFailure) return HandleFailure(result);

            return Ok(result);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCategoryListQuery(), cancellationToken);
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
