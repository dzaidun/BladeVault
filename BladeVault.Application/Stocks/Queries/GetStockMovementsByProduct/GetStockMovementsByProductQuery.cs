using BladeVault.Application.Common.Models;
using MediatR;

namespace BladeVault.Application.Stocks.Queries.GetStockMovementsByProduct
{
    public record GetStockMovementsByProductQuery(
        Guid ProductId,
        int Page = 1,
        int PageSize = 20)
        : IRequest<PagedResult<StockMovementDto>>;

    public record StockMovementDto(
        Guid Id,
        Guid ProductId,
        string MovementType,
        int Quantity,
        string Reason,
        string? DocumentReference,
        Guid? PerformedByUserId,
        DateTime CreatedAt);
}
