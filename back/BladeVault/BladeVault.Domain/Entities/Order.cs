using BladeVault.Domain.Common;
using BladeVault.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.New;
        public DeliveryMethod DeliveryMethod { get; private set; }
        public string DeliveryAddress { get; private set; } = string.Empty;
        public string? TrackingNumber { get; private set; }
        public string? Comment { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCallCenterOrder { get; private set; }

        // Navigation
        public User User { get; private set; } = null!;
        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();
        public Payment? Payment { get; private set; }

        protected Order() { }

        public static Order Create(
            Guid userId,
            DeliveryMethod deliveryMethod,
            string deliveryAddress,
            string? comment = null,
            bool isCallCenterOrder = false)
        {
            return new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = userId,
                DeliveryMethod = deliveryMethod,
                DeliveryAddress = deliveryAddress,
                Comment = comment,
                IsCallCenterOrder = isCallCenterOrder
            };
        }

        public Result AddItem(OrderItem item)
        {
            Items.Add(item);
            RecalculateTotal();
            return Result.Success();
        }

        public Result ChangeStatus(OrderStatus newStatus)
        {
            // Валідація переходу між статусами
            var allowedTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
        {
            { OrderStatus.New, new() { OrderStatus.Confirmed, OrderStatus.Cancelled } },
            { OrderStatus.Confirmed, new() { OrderStatus.InAssembly, OrderStatus.Cancelled } },
            { OrderStatus.InAssembly, new() { OrderStatus.ReadyToShip } },
            { OrderStatus.ReadyToShip, new() { OrderStatus.Shipped } },
            { OrderStatus.Shipped, new() { OrderStatus.Delivered, OrderStatus.Returned } },
            { OrderStatus.Delivered, new() { OrderStatus.Returned } },
        };

            if (!allowedTransitions.TryGetValue(Status, out var allowed) || !allowed.Contains(newStatus))
                return Result.Failure($"Неможливо змінити статус з {Status} на {newStatus}");

            Status = newStatus;
            SetUpdatedAt();
            return Result.Success();
        }

        public void SetTrackingNumber(string trackingNumber)
        {
            TrackingNumber = trackingNumber;
            SetUpdatedAt();
        }

        private void RecalculateTotal()
        {
            TotalAmount = Items.Sum(i => i.UnitPrice * i.Quantity);
        }

        private static string GenerateOrderNumber()
        {
            return $"BV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        }
    }
}
