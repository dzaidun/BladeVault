using BladeVault.Application.Orders.Commands.ChangeOrderStatus;
using BladeVault.Application.Orders.Commands.CreateOrder;
using BladeVault.Application.Orders.Queries.GetOrderById;
using BladeVault.Domain.Enums;
using BladeVault.WebAPI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BladeVault.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ISender _sender;

        public OrdersController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Отримати замовлення по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetOrderByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Створити замовлення (авторизований користувач)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateOrderCommand command,
            CancellationToken cancellationToken)
        {
            var id = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetOrderById), new { id }, new { id });
        }

        /// <summary>
        /// Змінити статус замовлення (тільки Owner/Admin)
        /// </summary>
        [HttpPatch("{id:guid}/status")]
        [Authorize(Policy = AuthorizationPolicies.OrderStatusManagement)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeOrderStatus(
            Guid id,
            [FromBody] OrderStatus newStatus,
            CancellationToken cancellationToken)
        {
            await _sender.Send(new ChangeOrderStatusCommand(id, newStatus), cancellationToken);
            return NoContent();
        }
    }
}
