using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Queries.GetKnifeById
{
    public record KnifeDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string SKU { get; init; } = string.Empty;
        public string Brand { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public string CountryOfOrigin { get; init; } = string.Empty;
        public string? Description { get; init; }

        // Ціна
        public decimal Price { get; init; }
        public decimal? DiscountPrice { get; init; }
        public decimal ActualPrice { get; init; }

        // Фізичні параметри
        public double? WeightGrams { get; init; }
        public double? OverallLengthMm { get; init; }
        public double? ClosedLengthMm { get; init; }

        // Клинок
        public string SteelType { get; init; } = string.Empty;
        public double BladeLengthMm { get; init; }
        public double BladeThicknessMm { get; init; }
        public string BladeShape { get; init; } = string.Empty;
        public string EdgeType { get; init; } = string.Empty;
        public bool IsCoated { get; init; }
        public string? CoatingType { get; init; }

        // Руків'я
        public string HandleMaterial { get; init; } = string.Empty;
        public double? HandleLengthMm { get; init; }

        // Механізм
        public string KnifeType { get; init; } = string.Empty;
        public string? LockType { get; init; }
        public string? OpeningMechanism { get; init; }

        // Додатково
        public bool HasClip { get; init; }
        public bool HasGuard { get; init; }
        public bool HasPommel { get; init; }
        public bool IncludesSheath { get; init; }
        public string? SheathMaterial { get; init; }

        // Категорія та залишки
        public string CategoryName { get; init; } = string.Empty;
        public int AvailableQuantity { get; init; }

        // Фото
        public IReadOnlyList<string> ImageUrls { get; init; } = [];
        public string? MainImageUrl { get; init; }

        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
