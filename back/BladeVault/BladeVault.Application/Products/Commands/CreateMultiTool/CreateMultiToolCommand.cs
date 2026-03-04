using BladeVault.Domain.Enums.ProductSpecs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Commands.CreateMultiTool
{
    public record CreateMultiToolCommand : IRequest<Guid>
    {
        // Базові поля
        public string Name { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string SKU { get; init; } = string.Empty;
        public string Brand { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public string CountryOfOrigin { get; init; } = string.Empty;
        public Guid CategoryId { get; init; }
        public decimal Price { get; init; }
        public decimal? DiscountPrice { get; init; }
        public string? Description { get; init; }

        // Фізичні параметри
        public double? WeightGrams { get; init; }
        public double? OverallLengthMm { get; init; }
        public double? ClosedLengthMm { get; init; }

        // Специфічні параметри мультитула
        public string HandleMaterial { get; init; } = string.Empty;
        public bool HasPliers { get; init; }
        public bool ReplaceableWireCutters { get; init; }
        public bool HasLocking { get; init; }
        public bool IsOneHandOpenable { get; init; }
        public bool IncludesPouch { get; init; }
        public string? PouchMaterial { get; init; }
        public bool HasBitSet { get; init; }
        public int? BitCount { get; init; }

        // Список інструментів
        public IReadOnlyList<ToolComponentDto> IncludedTools { get; init; } = [];
    }

    // Вкладений DTO для інструментів які передаємо при створенні
    public record ToolComponentDto(ToolType Type, string? Description);
}
