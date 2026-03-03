using BladeVault.Application.CallCenter.Commands.CreateCallLog;
using BladeVault.Application.CallCenter.Queries.GetCallLogsByCustomer;
using BladeVault.Domain.Enums;
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
    public class CallCenterController : ControllerBase
    {
        private readonly ISender _sender;

        public CallCenterController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Додати запис дзвінка клієнту
        /// </summary>
        [HttpPost("logs")]
        [Authorize(Policy = AuthorizationPolicies.CallCenterOperations)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateCallLog(
            [FromBody] CreateCallLogCommand command,
            CancellationToken cancellationToken)
        {
            var id = await _sender.Send(command with { PerformedByUserId = GetCurrentUserId() }, cancellationToken);
            return CreatedAtAction(nameof(GetCallLogsByCustomer), new { customerId = command.CustomerId }, new { id });
        }

        /// <summary>
        /// Отримати історію дзвінків по клієнту
        /// </summary>
        [HttpGet("customers/{customerId:guid}/logs")]
        [Authorize(Policy = AuthorizationPolicies.CallCenterOperations)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetCallLogsByCustomer(
            Guid customerId,
            [FromQuery] CallStatus? status,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(
                new GetCallLogsByCustomerQuery(customerId, status, from, to, page, pageSize),
                cancellationToken);

            return Ok(result);
        }

        private Guid GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Користувач не авторизований");

            return Guid.Parse(claim);
        }
    }
}
