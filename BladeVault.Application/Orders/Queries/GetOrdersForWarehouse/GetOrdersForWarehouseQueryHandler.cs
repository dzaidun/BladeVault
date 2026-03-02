using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Orders.Queries.GetOrdersForWarehouse
{
    public class GetOrdersForWarehouseQueryHandler
        : IRequestHandler<GetOrdersForWarehouseQuery, IReadOnlyList<WarehouseOrderDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetOrdersForWarehouseQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<WarehouseOrderDto>> Handle(
            GetOrdersForWarehouseQuery request,
            CancellationToken cancellationToken)
        {
            var orders = await _uow.Orders.GetForWarehouseAsync(cancellationToken);

            return orders.Select(o => new WarehouseOrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Status = o.Status.ToString(),
                DeliveryAddress = o.DeliveryAddress,
                Comment = o.Comment,
                TotalAmount = o.TotalAmount,
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(i => new WarehouseOrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    ProductSKU = i.ProductSKU,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();
        }
    }
}
