using BladeVault.Domain.Enums.ProductSpecs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Commands.UpdateKnife
{
    public record UpdateKnifeCommand : IRequest
    {
        public Guid Id { get; init; }

        // Базові поля
        public string Name { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string Brand { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public string CountryOfOrigin { get; init; } = string.Empty;
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public decimal? DiscountPrice { get; init; }

        // Фізичні параметри
        public double? WeightGrams { get; init; }
        public double? OverallLengthMm { get; init; }
        public double? ClosedLengthMm { get; init; }

        // Клинок
        public string SteelType { get; init; } = string.Empty;
        public double BladeLengthMm { get; init; }
        public double BladeThicknessMm { get; init; }
        public BladeShape BladeShape { get; init; }
        public EdgeType EdgeType { get; init; }
        public bool IsCoated { get; init; }
        public string? CoatingType { get; init; }

        // Руків'я
        public string HandleMaterial { get; init; } = string.Empty;
        public double? HandleLengthMm { get; init; }

        // Механізм
        public LockType? LockType { get; init; }
        public OpeningMechanism? OpeningMechanism { get; init; }

        // Додатково
        public bool HasClip { get; init; }
        public bool HasGuard { get; init; }
        public bool HasPommel { get; init; }
        public bool IncludesSheath { get; init; }
        public string? SheathMaterial { get; init; }
    }
}
