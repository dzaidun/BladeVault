using BladeVault.Application.Orders.Commands.ChangeOrderStatus;
using BladeVault.Application.Orders.Queries.GetOrdersForWarehouse;
using BladeVault.Domain.Enums;
using BladeVault.WebAPI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BladeVault.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = AuthorizationPolicies.WarehouseOperations)]
    public class WarehouseController : ControllerBase
    {
        private readonly ISender _sender;

        public WarehouseController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Отримати замовлення для складу (підтверджені / у збірці)
        /// </summary>
        [HttpGet("orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetOrdersForWarehouse(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetOrdersForWarehouseQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Позначити замовлення як "У збірці"
        /// </summary>
        [HttpPost("orders/{id:guid}/start-assembly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StartAssembly(Guid id, CancellationToken cancellationToken)
        {
            await _sender.Send(new ChangeOrderStatusCommand(id, OrderStatus.InAssembly), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Позначити замовлення як "Готово до відправки"
        /// </summary>
        [HttpPost("orders/{id:guid}/ready-to-ship")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkReadyToShip(Guid id, CancellationToken cancellationToken)
        {
            await _sender.Send(new ChangeOrderStatusCommand(id, OrderStatus.ReadyToShip), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Позначити замовлення як "Відправлено"
        /// </summary>
        [HttpPost("orders/{id:guid}/ship")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkShipped(
            Guid id,
            [FromBody] ShipOrderRequest? request,
            CancellationToken cancellationToken)
        {
            await _sender.Send(
                new ChangeOrderStatusCommand(id, OrderStatus.Shipped, request?.TrackingNumber),
                cancellationToken);

            return NoContent();
        }

        public record ShipOrderRequest(string? TrackingNumber);
    }
}
