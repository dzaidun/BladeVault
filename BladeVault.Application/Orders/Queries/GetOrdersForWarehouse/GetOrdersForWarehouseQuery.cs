using MediatR;

namespace BladeVault.Application.Orders.Queries.GetOrdersForWarehouse
{
    public record GetOrdersForWarehouseQuery : IRequest<IReadOnlyList<WarehouseOrderDto>>;

    public record WarehouseOrderDto
    {
        public Guid Id { get; init; }
        public string OrderNumber { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string DeliveryAddress { get; init; } = string.Empty;
        public string? Comment { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime CreatedAt { get; init; }
        public IReadOnlyList<WarehouseOrderItemDto> Items { get; init; } = [];
    }

    public record WarehouseOrderItemDto
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public string ProductSKU { get; init; } = string.Empty;
        public int Quantity { get; init; }
    }
}
