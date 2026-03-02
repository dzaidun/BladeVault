using BladeVault.Domain.Common;
using BladeVault.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities
{
    public class Stock : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public int ReservedQuantity { get; private set; }

        public int AvailableQuantity => Quantity - ReservedQuantity;

        // Navigation
        public Product Product { get; private set; } = null!;

        protected Stock() { }

        public static Stock Create(Guid productId, int quantity = 0)
        {
            return new Stock
            {
                ProductId = productId,
                Quantity = quantity
            };
        }

        public Result AddStock(int amount)
        {
            if (amount <= 0) return Result.Failure("Кількість має бути більше 0");
            Quantity += amount;
            SetUpdatedAt();
            return Result.Success();
        }

        public Result RemoveStock(int amount)
        {
            if (amount <= 0) return Result.Failure("Кількість має бути більше 0");
            if (AvailableQuantity < amount) return Result.Failure("Недостатньо вільного товару на складі");
            Quantity -= amount;
            SetUpdatedAt();
            return Result.Success();
        }

        public Result Reserve(int amount)
        {
            if (amount <= 0) return Result.Failure("Кількість має бути більше 0");
            if (AvailableQuantity < amount) return Result.Failure("Недостатньо товару на складі");
            ReservedQuantity += amount;
            SetUpdatedAt();
            return Result.Success();
        }

        public Result Release(int amount)
        {
            if (amount <= 0) return Result.Failure("Кількість має бути більше 0");
            if (ReservedQuantity < amount) return Result.Failure("Неможливо звільнити більше ніж зарезервовано");
            ReservedQuantity -= amount;
            SetUpdatedAt();
            return Result.Success();
        }

        public Result WriteOff(int amount)
        {
            if (amount <= 0) return Result.Failure("Кількість має бути більше 0");
            if (ReservedQuantity < amount) return Result.Failure("Неможливо списати більше ніж зарезервовано");
            ReservedQuantity -= amount;
            Quantity -= amount;
            SetUpdatedAt();
            return Result.Success();
        }
    }
}
