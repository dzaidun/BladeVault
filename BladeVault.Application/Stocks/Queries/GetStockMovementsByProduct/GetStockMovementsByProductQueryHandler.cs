using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Stocks.Queries.GetStockMovementsByProduct
{
    public class GetStockMovementsByProductQueryHandler
        : IRequestHandler<GetStockMovementsByProductQuery, IReadOnlyList<StockMovementDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetStockMovementsByProductQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<StockMovementDto>> Handle(
            GetStockMovementsByProductQuery query,
            CancellationToken cancellationToken)
        {
            var movements = await _uow.StockMovements.GetByProductIdAsync(query.ProductId, cancellationToken);

            return movements
                .Select(m => new StockMovementDto(
                    Id: m.Id,
                    ProductId: m.ProductId,
                    MovementType: m.MovementType.ToString(),
                    Quantity: m.Quantity,
                    Reason: m.Reason,
                    DocumentReference: m.DocumentReference,
                    PerformedByUserId: m.PerformedByUserId,
                    CreatedAt: m.CreatedAt))
                .ToList();
        }
    }
}
