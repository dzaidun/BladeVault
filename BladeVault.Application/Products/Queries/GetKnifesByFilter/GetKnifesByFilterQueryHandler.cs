using BladeVault.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Queries.GetKnifesByFilter
{
    public class GetKnifesByFilterQueryHandler
    : IRequestHandler<GetKnifesByFilterQuery, IReadOnlyList<KnifeListItemDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetKnifesByFilterQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<KnifeListItemDto>> Handle(
            GetKnifesByFilterQuery query,
            CancellationToken cancellationToken)
        {
            var knives = await _uow.Knives.GetByFiltersAsync(
                knifeType: query.KnifeType,
                steelType: query.SteelType,
                bladeShape: query.BladeShape,
                edgeType: query.EdgeType,
                lockType: query.LockType,
                openingMechanism: query.OpeningMechanism,
                hasClip: query.HasClip,
                minPrice: query.MinPrice,
                maxPrice: query.MaxPrice,
                maxBladeLengthMm: query.MaxBladeLengthMm,
                cancellationToken: cancellationToken);

            return knives.Select(knife => new KnifeListItemDto
            {
                Id = knife.Id,
                Name = knife.Name,
                Slug = knife.Slug,
                Brand = knife.Brand,
                Model = knife.Model,
                Price = knife.Price,
                DiscountPrice = knife.DiscountPrice,
                ActualPrice = knife.GetActualPrice(),
                SteelType = knife.SteelType,
                BladeLengthMm = knife.BladeLengthMm,
                BladeShape = knife.BladeShape.ToString(),
                KnifeType = knife.KnifeType.ToString(),
                LockType = knife.LockType?.ToString(),
                HasClip = knife.HasClip,
                AvailableQuantity = knife.Stock?.AvailableQuantity ?? 0,
                MainImageUrl = knife.Images.FirstOrDefault(i => i.IsMain)?.Url
            }).ToList();
        }
    }
}
