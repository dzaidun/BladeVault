using BladeVault.Application.Users.Commands.ChangePassword;
using BladeVault.Application.Users.Commands.ChangeTemporaryPassword;
using BladeVault.Application.Users.Commands.CreateStaffUser;
using BladeVault.Application.Users.Commands.DeactivateUser;
using BladeVault.Application.Users.Commands.DeleteUser;
using BladeVault.Application.Users.Commands.UpdateUserProfile;
using BladeVault.Application.Users.Commands.UpdateStaffUser;
using BladeVault.Application.Users.Queries.GetUserProfile;
using BladeVault.Application.Users.Queries.GetUsersList;
using BladeVault.WebAPI.Authorization;
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

        // ── Profile Management ─────────────────────────────────────────────
        // Used for self-profile operations by any authenticated user

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
        /// Змінити профіль поточного користувача
        /// </summary>
        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] UpdateUserProfileCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command with { UserId = GetCurrentUserId() }, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Змінити пароль користувача
        /// </summary>
        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command with { UserId = GetCurrentUserId() }, cancellationToken);
            return NoContent();
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

        // ── Staff Management (Owner/Admin only) ──────────────────────────
        // Used for viewing and managing staff users

        /// <summary>
        /// Отримати список користувачів (для Owner/Admin)
        /// </summary>
        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.OwnerOrAdmin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsersList(
            [FromQuery] string? search,
            [FromQuery] string? role,
            [FromQuery] bool? isActive,
            [FromQuery] string? sortBy = "createdAt",
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(
                new GetUsersListQuery(search, role, isActive, sortBy, sortOrder, page, pageSize),
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Отримати профіль користувача по Id (тільки Owner/Admin)
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.OwnerOrAdmin)]
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
        [Authorize(Policy = AuthorizationPolicies.OwnerOrAdmin)]
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
        /// Змінити дані працівника (тільки Owner/Admin)
        /// </summary>
        [HttpPut("staff/{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.OwnerOrAdmin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStaffUser(
            Guid id,
            [FromBody] UpdateStaffUserCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command with { StaffUserId = id }, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Деактивувати користувача (тільки Owner/Admin)
        /// </summary>
        [HttpPost("staff/{id:guid}/deactivate")]
        [Authorize(Policy = AuthorizationPolicies.OwnerOrAdmin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateUser(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _sender.Send(new DeactivateUserCommand(id), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Видалити користувача (тільки Owner/Admin)
        /// </summary>
        [HttpDelete("staff/{id:guid}")]
        [Authorize(Policy = AuthorizationPolicies.OwnerOrAdmin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _sender.Send(new DeleteUserCommand(id), cancellationToken);
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
