using BladeVault.Domain.Enums.ProductSpecs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Queries.GetKnifesByFilter
{
    public record GetKnifesByFilterQuery : IRequest<IReadOnlyList<KnifeListItemDto>>
    {
        public KnifeType? KnifeType { get; init; }
        public string? SteelType { get; init; }
        public BladeShape? BladeShape { get; init; }
        public EdgeType? EdgeType { get; init; }
        public LockType? LockType { get; init; }
        public OpeningMechanism? OpeningMechanism { get; init; }
        public bool? HasClip { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public double? MaxBladeLengthMm { get; init; }
    }
}
