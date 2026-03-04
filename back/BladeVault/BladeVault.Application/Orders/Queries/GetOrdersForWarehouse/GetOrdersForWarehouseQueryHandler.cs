using BladeVault.Application.Common.Models;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Orders.Queries.GetOrdersForWarehouse
{
    public class GetOrdersForWarehouseQueryHandler
        : IRequestHandler<GetOrdersForWarehouseQuery, PagedResult<WarehouseOrderDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetOrdersForWarehouseQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedResult<WarehouseOrderDto>> Handle(
            GetOrdersForWarehouseQuery request,
            CancellationToken cancellationToken)
        {
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize is < 1 or > 200 ? 20 : request.PageSize;

            var orders = await _uow.Orders.GetForWarehouseAsync(cancellationToken);

            var filteredOrders = request.Status.HasValue
                ? orders.Where(o => o.Status == request.Status.Value).ToList()
                : orders;

            var totalCount = filteredOrders.Count;

            var items = filteredOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new WarehouseOrderDto
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
                })
                .ToList();

            return new PagedResult<WarehouseOrderDto>(items, totalCount, page, pageSize);
        }
    }
}
