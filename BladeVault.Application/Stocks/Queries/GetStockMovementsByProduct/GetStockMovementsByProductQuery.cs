using MediatR;

namespace BladeVault.Application.Stocks.Queries.GetStockMovementsByProduct
{
    public record GetStockMovementsByProductQuery(Guid ProductId)
        : IRequest<IReadOnlyList<StockMovementDto>>;

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
