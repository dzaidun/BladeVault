using BladeVault.Domain.Common;
using BladeVault.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
        public string? ExternalPaymentId { get; private set; }
        public string? PaymentMethod { get; private set; }
        public DateTime? PaidAt { get; private set; }

        // Navigation
        public Order Order { get; private set; } = null!;

        protected Payment() { }

        public static Payment Create(Guid orderId, decimal amount, string? paymentMethod = null)
        {
            return new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = paymentMethod
            };
        }

        public void MarkAsPaid(string externalPaymentId)
        {
            Status = PaymentStatus.Paid;
            ExternalPaymentId = externalPaymentId;
            PaidAt = DateTime.UtcNow;
            SetUpdatedAt();
        }

        public void MarkAsFailed() { Status = PaymentStatus.Failed; SetUpdatedAt(); }
        public void MarkAsRefunded() { Status = PaymentStatus.Refunded; SetUpdatedAt(); }
    }
}
