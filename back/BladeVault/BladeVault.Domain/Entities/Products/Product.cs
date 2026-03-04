using BladeVault.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Entities.Products
{
    public abstract class Product : BaseEntity
    {
        // --- Основна інформація ---
        public string Name { get; protected set; } = string.Empty;
        public string Slug { get; protected set; } = string.Empty;
        public string? Description { get; protected set; }
        public string SKU { get; protected set; } = string.Empty;
        public string Brand { get; protected set; } = string.Empty;
        public string Model { get; protected set; } = string.Empty;
        public string CountryOfOrigin { get; protected set; } = string.Empty;

        // --- Ціна ---
        public decimal Price { get; protected set; }
        public decimal? DiscountPrice { get; protected set; }

        // --- Спільні фізичні параметри ---
        public double? WeightGrams { get; protected set; }
        public double? OverallLengthMm { get; protected set; }
        public double? ClosedLengthMm { get; protected set; }  // Для складних ножів та мультитулів

        // --- Статус ---
        public bool IsActive { get; protected set; } = true;

        // --- Зв'язки ---
        public Guid CategoryId { get; protected set; }

        // --- Navigation properties ---
        public Category Category { get; protected set; } = null!;
        public Stock Stock { get; protected set; } = null!;
        public ICollection<ProductImage> Images { get; protected set; } = new List<ProductImage>();
        public ICollection<OrderItem> OrderItems { get; protected set; } = new List<OrderItem>();

        // --- Методи ---
        public decimal GetActualPrice() => DiscountPrice ?? Price;

        public void UpdatePrice(decimal price, decimal? discountPrice = null)
        {
            if (price <= 0) throw new ArgumentException("Ціна має бути більше 0");
            if (discountPrice.HasValue && discountPrice >= price)
                throw new ArgumentException("Ціна зі знижкою має бути менша за звичайну ціну");

            Price = price;
            DiscountPrice = discountPrice;
            SetUpdatedAt();
        }

        public void UpdateBaseInfo(
            string name,
            string slug,
            string brand,
            string model,
            string countryOfOrigin,
            string? description,
            double? weightGrams,
            double? overallLengthMm,
            double? closedLengthMm)
        {
            Name = name;
            Slug = slug;
            Brand = brand;
            Model = model;
            CountryOfOrigin = countryOfOrigin;
            Description = description;
            WeightGrams = weightGrams;
            OverallLengthMm = overallLengthMm;
            ClosedLengthMm = closedLengthMm;
            SetUpdatedAt();
        }

        public void Activate() { IsActive = true; SetUpdatedAt(); }
        public void Deactivate() { IsActive = false; SetUpdatedAt(); }
    }
}
