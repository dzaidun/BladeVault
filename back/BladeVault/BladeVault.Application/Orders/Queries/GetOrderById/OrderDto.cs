using BladeVault.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Orders.Queries.GetOrderById
{
    public record OrderDto
    {
        public Guid Id { get; init; }
        public string OrderNumber { get; init; } = string.Empty;
        public OrderStatus Status { get; init; }
        public DeliveryMethod DeliveryMethod { get; init; }
        public string DeliveryAddress { get; init; } = string.Empty; // ← замість NovaPostWarehouse
        public string? TrackingNumber { get; init; }
        public string? Comment { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }

        public Guid UserId { get; init; }
        public string UserFullName { get; init; } = string.Empty;
        public string UserEmail { get; init; } = string.Empty;
        public string UserPhone { get; init; } = string.Empty;

        public IReadOnlyList<OrderItemDto> Items { get; init; } = [];
        public OrderPaymentDto? Payment { get; init; }
    }

    public record OrderItemDto
    {
        public Guid Id { get; init; }
        public Guid ProductId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public string ProductSKU { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal TotalPrice { get; init; }
    }

    public record OrderPaymentDto
    {
        public Guid Id { get; init; }
        public decimal Amount { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? PaymentMethod { get; init; }
        public DateTime? PaidAt { get; init; }
    }
}
