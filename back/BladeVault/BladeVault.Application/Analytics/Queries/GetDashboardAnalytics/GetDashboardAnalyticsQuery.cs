using MediatR;

namespace BladeVault.Application.Analytics.Queries.GetDashboardAnalytics
{
    public record GetDashboardAnalyticsQuery(DateTime? From, DateTime? To) : IRequest<DashboardAnalyticsDto>;

    public record DashboardAnalyticsDto
    {
        public DateTime From { get; init; }
        public DateTime To { get; init; }
        public int TotalOrders { get; init; }
        public int DeliveredOrders { get; init; }
        public int CancelledOrders { get; init; }
        public decimal Revenue { get; init; }
        public IReadOnlyList<OrderStatusStatDto> StatusStats { get; init; } = [];
        public IReadOnlyList<TopProductStatDto> TopProducts { get; init; } = [];
        public IReadOnlyList<LowStockStatDto> LowStock { get; init; } = [];
    }

    public record OrderStatusStatDto(string Status, int Count);
    public record TopProductStatDto(Guid ProductId, string ProductName, string ProductSku, int QuantitySold, decimal Revenue);
    public record LowStockStatDto(Guid ProductId, int AvailableQuantity);
}
