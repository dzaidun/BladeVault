using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Queries.GetKnifeById
{
    public class GetKnifeByIdQueryHandler : IRequestHandler<GetKnifeByIdQuery, KnifeDto>
    {
        private readonly IUnitOfWork _uow;

        public GetKnifeByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<KnifeDto> Handle(
            GetKnifeByIdQuery query,
            CancellationToken cancellationToken)
        {
            var knife = await _uow.Knives.GetByIdAsync(query.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Knife), query.Id);

            return new KnifeDto
            {
                Id = knife.Id,
                Name = knife.Name,
                Slug = knife.Slug,
                SKU = knife.SKU,
                Brand = knife.Brand,
                Model = knife.Model,
                CountryOfOrigin = knife.CountryOfOrigin,
                Description = knife.Description,

                Price = knife.Price,
                DiscountPrice = knife.DiscountPrice,
                ActualPrice = knife.GetActualPrice(),

                WeightGrams = knife.WeightGrams,
                OverallLengthMm = knife.OverallLengthMm,
                ClosedLengthMm = knife.ClosedLengthMm,

                SteelType = knife.SteelType,
                BladeLengthMm = knife.BladeLengthMm,
                BladeThicknessMm = knife.BladeThicknessMm,
                BladeShape = knife.BladeShape.ToString(),
                EdgeType = knife.EdgeType.ToString(),
                IsCoated = knife.IsCoated,
                CoatingType = knife.CoatingType,

                HandleMaterial = knife.HandleMaterial,
                HandleLengthMm = knife.HandleLengthMm,

                KnifeType = knife.KnifeType.ToString(),
                LockType = knife.LockType?.ToString(),
                OpeningMechanism = knife.OpeningMechanism?.ToString(),

                HasClip = knife.HasClip,
                HasGuard = knife.HasGuard,
                HasPommel = knife.HasPommel,
                IncludesSheath = knife.IncludesSheath,
                SheathMaterial = knife.SheathMaterial,

                CategoryName = knife.Category?.Name ?? string.Empty,
                AvailableQuantity = knife.Stock?.AvailableQuantity ?? 0,

                ImageUrls = knife.Images
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.Url)
                    .ToList(),
                MainImageUrl = knife.Images
                    .FirstOrDefault(i => i.IsMain)?.Url,

                IsActive = knife.IsActive,
                CreatedAt = knife.CreatedAt,
                UpdatedAt = knife.UpdatedAt
            };
        }
    }
}
