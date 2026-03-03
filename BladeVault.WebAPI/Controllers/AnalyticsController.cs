using BladeVault.Application.Analytics.Queries.GetDashboardAnalytics;
using BladeVault.WebAPI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BladeVault.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = AuthorizationPolicies.AnalyticsRead)]
    public class AnalyticsController : ControllerBase
    {
        private readonly ISender _sender;

        public AnalyticsController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Дашборд аналітики продажів та складу за період
        /// </summary>
        [HttpGet("dashboard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDashboard(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetDashboardAnalyticsQuery(from, to), cancellationToken);
            return Ok(result);
        }
    }
}
