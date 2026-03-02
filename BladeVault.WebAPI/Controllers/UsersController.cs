using BladeVault.Application.Users.Commands.ChangeTemporaryPassword;
using BladeVault.Application.Users.Commands.CreateStaffUser;
using BladeVault.Application.Users.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BladeVault.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Отримати профіль поточного користувача
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var result = await _sender.Send(new GetUserProfileQuery(userId), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Отримати профіль користувача по Id (тільки Owner/Admin)
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Owner,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUserProfileQuery(id), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Створити працівника (тільки Owner/Admin)
        /// </summary>
        [HttpPost("staff")]
        [Authorize(Roles = "Owner,Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateStaffUser(
            [FromBody] CreateStaffUserCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command with { CreatedByUserId = GetCurrentUserId() }, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = result.UserId }, result);
        }

        /// <summary>
        /// Змінити тимчасовий пароль після першого входу
        /// </summary>
        [HttpPost("change-temporary-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeTemporaryPassword(
            [FromBody] ChangeTemporaryPasswordCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command with { UserId = GetCurrentUserId() }, cancellationToken);
            return NoContent();
        }

        // ── Helpers ───────────────────────────────────────────────
        private Guid GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Користувач не авторизований");

            return Guid.Parse(claim);
        }
    }
}
