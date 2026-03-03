using BladeVault.Domain.Enums;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Analytics.Queries.GetDashboardAnalytics
{
    public class GetDashboardAnalyticsQueryHandler
        : IRequestHandler<GetDashboardAnalyticsQuery, DashboardAnalyticsDto>
    {
        private readonly IUnitOfWork _uow;

        public GetDashboardAnalyticsQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<DashboardAnalyticsDto> Handle(GetDashboardAnalyticsQuery query, CancellationToken cancellationToken)
        {
            var to = query.To ?? DateTime.UtcNow;
            var from = query.From ?? to.AddDays(-30);

            var orders = await _uow.Orders.GetForAnalyticsAsync(from, to, cancellationToken);
            var stocks = await _uow.Stock.GetAllAsync(cancellationToken);
            var products = await _uow.Products.GetAllAsync(cancellationToken);
            var productMap = products.ToDictionary(x => x.Id);

            var deliveredOrders = orders.Where(x => x.Status == OrderStatus.Delivered).ToList();

            var topProducts = deliveredOrders
                .SelectMany(x => x.Items)
                .GroupBy(x => x.ProductId)
                .Select(g =>
                {
                    var sample = g.First();
                    var productName = productMap.TryGetValue(g.Key, out var p) ? p.Name : sample.ProductName;
                    var productSku = productMap.TryGetValue(g.Key, out p) ? p.SKU : sample.ProductSKU;

                    return new TopProductStatDto(
                        ProductId: g.Key,
                        ProductName: productName,
                        ProductSku: productSku,
                        QuantitySold: g.Sum(i => i.Quantity),
                        Revenue: g.Sum(i => i.Quantity * i.UnitPrice));
                })
                .OrderByDescending(x => x.QuantitySold)
                .ThenByDescending(x => x.Revenue)
                .Take(10)
                .ToList();

            var statusStats = orders
                .GroupBy(x => x.Status)
                .Select(g => new OrderStatusStatDto(g.Key.ToString(), g.Count()))
                .OrderByDescending(x => x.Count)
                .ToList();

            var lowStock = stocks
                .Where(x => x.AvailableQuantity <= 5)
                .OrderBy(x => x.AvailableQuantity)
                .Select(x => new LowStockStatDto(x.ProductId, x.AvailableQuantity))
                .Take(20)
                .ToList();

            return new DashboardAnalyticsDto
            {
                From = from,
                To = to,
                TotalOrders = orders.Count,
                DeliveredOrders = deliveredOrders.Count,
                CancelledOrders = orders.Count(x => x.Status == OrderStatus.Cancelled),
                Revenue = deliveredOrders.Sum(x => x.TotalAmount),
                StatusStats = statusStats,
                TopProducts = topProducts,
                LowStock = lowStock
            };
        }
    }
}
