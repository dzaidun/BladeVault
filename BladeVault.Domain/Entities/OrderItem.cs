using BladeVault.Domain.Common;
using BladeVault.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public string ProductSKU { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public decimal TotalPrice => UnitPrice * Quantity;

        // Navigation
        public Order Order { get; private set; } = null!;
        public Product Product { get; private set; } = null!;

        protected OrderItem() { }

        public static OrderItem Create(
            Guid orderId,
            Guid productId,
            string productName,
            string productSKU,
            int quantity,
            decimal unitPrice)
        {
            return new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                ProductName = productName,
                ProductSKU = productSKU,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
        }
    }
}
