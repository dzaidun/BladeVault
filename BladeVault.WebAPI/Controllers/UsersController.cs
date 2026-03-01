using BladeVault.Application.Users.Queries.GetUserProfile;
using BladeVault.Domain.Entities;
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
        /// Отримати профіль користувача по Id (тільки Admin)
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
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

        // ── Helpers ───────────────────────────────────────────────
        private Guid GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Користувач не авторизований");

            return Guid.Parse(claim);
        }
    }
}
