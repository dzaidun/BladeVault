using BladeVault.Application.Common.Models;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Stocks.Queries.GetStockMovementsByProduct
{
    public class GetStockMovementsByProductQueryHandler
        : IRequestHandler<GetStockMovementsByProductQuery, PagedResult<StockMovementDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetStockMovementsByProductQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<StockMovementDto>> Handle(
            GetStockMovementsByProductQuery query,
            CancellationToken cancellationToken)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize is < 1 or > 200 ? 20 : query.PageSize;

            var movements = await _uow.StockMovements.GetByProductIdAsync(query.ProductId, cancellationToken);

            var totalCount = movements.Count;
            var items = movements
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            return new PagedResult<StockMovementDto>(items, totalCount, page, pageSize);
        }
    }
}
