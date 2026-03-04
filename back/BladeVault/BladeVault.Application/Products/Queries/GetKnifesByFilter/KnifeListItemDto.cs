using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Queries.GetKnifesByFilter
{
    public record KnifeListItemDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string Brand { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public decimal? DiscountPrice { get; init; }
        public decimal ActualPrice { get; init; }
        public string SteelType { get; init; } = string.Empty;
        public double BladeLengthMm { get; init; }
        public string BladeShape { get; init; } = string.Empty;
        public string KnifeType { get; init; } = string.Empty;
        public string? LockType { get; init; }
        public bool HasClip { get; init; }
        public int AvailableQuantity { get; init; }
        public string? MainImageUrl { get; init; }
    }
}
