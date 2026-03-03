using BladeVault.Application.Stocks.Commands.ChangeStockBalance;
using BladeVault.Application.Stocks.Queries.GetStockMovementsByProduct;
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
    public class StockController : ControllerBase
    {
        private readonly ISender _sender;

        public StockController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Змінити баланс складу (додавання/списання) з аудитом
        /// </summary>
        [HttpPost("change-balance")]
        [Authorize(Policy = AuthorizationPolicies.StockManagement)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeBalance(
            [FromBody] ChangeStockBalanceCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command with { PerformedByUserId = GetCurrentUserId() }, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Отримати історію змін складу по товару
        /// </summary>
        [HttpGet("{productId:guid}/movements")]
        [Authorize(Policy = AuthorizationPolicies.StockRead)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetMovementsByProduct(
            Guid productId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetStockMovementsByProductQuery(productId, page, pageSize), cancellationToken);
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
