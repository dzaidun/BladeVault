using BladeVault.Application.Products.Commands.CreateKnife;
using BladeVault.Application.Products.Commands.DeleteProduct;
using BladeVault.Application.Products.Commands.UpdateKnife;
using BladeVault.Application.Products.Queries.GetKnifeById;
using BladeVault.Application.Products.Queries.GetKnifesByFilter;
using BladeVault.Domain.Enums.ProductSpecs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BladeVault.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductsController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Отримати список ножів з фільтрами
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetKnives(
            [FromQuery] KnifeType? knifeType,
            [FromQuery] string? steelType,
            [FromQuery] BladeShape? bladeShape,
            [FromQuery] EdgeType? edgeType,
            [FromQuery] LockType? lockType,
            [FromQuery] OpeningMechanism? openingMechanism,
            [FromQuery] bool? hasClip,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] double? maxBladeLengthMm,
            CancellationToken cancellationToken)
        {
            var query = new GetKnifesByFilterQuery
            {
                KnifeType = knifeType,
                SteelType = steelType,
                BladeShape = bladeShape,
                EdgeType = edgeType,
                LockType = lockType,
                OpeningMechanism = openingMechanism,
                HasClip = hasClip,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MaxBladeLengthMm = maxBladeLengthMm
            };

            var result = await _sender.Send(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Отримати ніж по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetKnifeById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetKnifeByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Створити ніж (тільки Admin)
        /// </summary>
        [HttpPost("knife")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateKnife(
            [FromBody] CreateKnifeCommand command,
            CancellationToken cancellationToken)
        {
            var id = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetKnifeById), new { id }, new { id });
        }

        /// <summary>
        /// Оновити ніж (тільки Admin)
        /// </summary>
        [HttpPut("knife/{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateKnife(
            Guid id,
            [FromBody] UpdateKnifeCommand command,
            CancellationToken cancellationToken)
        {
            var updatedCommand = command with { Id = id };
            await _sender.Send(updatedCommand, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Видалити продукт (soft delete, тільки Admin)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _sender.Send(new DeleteProductCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
