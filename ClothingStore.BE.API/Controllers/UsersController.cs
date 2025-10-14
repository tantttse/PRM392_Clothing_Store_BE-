using System.Threading;
using System.Threading.Tasks;
using ClothingStore.Application.Features.User.Commands.RegisterUser;
using ClothingStore.Application.Features.User.Commands.Login;
// using ClothingStore.Application.Features.User.Commands.RefreshToken;
using ClothingStore.Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Common.ResponseModel;
using Shared.Application.Abstractions.Adapters;
using Shared.Application.Abstractions.Authentication;
using Shared.Presentation.Common;
using Shared.Application.Common.Commands;

namespace ClothingStore.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("register")]
        // [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
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

        [HttpPost("login")]
        // [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
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
            var result = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);
            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result);
        }

        // [HttpGet("all")]
        // // [Authorize("Admin")]
        // public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        // {
        //     var result = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
        //     return Ok(result);
        // }

        [HttpGet("health")]
        // [AllowAnonymous]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy" });
        }
    }
}
