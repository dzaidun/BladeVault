using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Commands.CreateKnife
{
    public class CreateKnifeCommandHandler : IRequestHandler<CreateKnifeCommand, Guid>
    {
        private readonly IUnitOfWork _uow;

        public CreateKnifeCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Handle(
            CreateKnifeCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Перевіряємо чи існує категорія
            var category = await _uow.Categories.GetByIdAsync(command.CategoryId, cancellationToken)
                ?? throw new NotFoundException(nameof(Category), command.CategoryId);

            // 2. Перевіряємо унікальність SKU і Slug
            if (!await _uow.Products.IsSkuUniqueAsync(command.SKU, cancellationToken))
                throw new ValidationException(new Dictionary<string, string[]>
            {
                { nameof(command.SKU), ["Товар з таким SKU вже існує"] }
            });

            if (!await _uow.Products.IsSlugUniqueAsync(command.Slug, cancellationToken))
                throw new ValidationException(new Dictionary<string, string[]>
            {
                { nameof(command.Slug), ["Товар з таким Slug вже існує"] }
            });

            // 3. Створюємо ніж через доменний метод
            var result = Knife.Create(
                name: command.Name,
                slug: command.Slug,
                sku: command.SKU,
                brand: command.Brand,
                model: command.Model,
                countryOfOrigin: command.CountryOfOrigin,
                categoryId: command.CategoryId,
                price: command.Price,
                steelType: command.SteelType,
                bladeLengthMm: command.BladeLengthMm,
                bladeThicknessMm: command.BladeThicknessMm,
                bladeShape: command.BladeShape,
                edgeType: command.EdgeType,
                handleMaterial: command.HandleMaterial,
                knifeType: command.KnifeType,
                description: command.Description,
                weightGrams: command.WeightGrams,
                overallLengthMm: command.OverallLengthMm,
                closedLengthMm: command.ClosedLengthMm,
                handleLengthMm: command.HandleLengthMm,
                lockType: command.LockType,
                openingMechanism: command.OpeningMechanism,
                hasClip: command.HasClip,
                hasGuard: command.HasGuard,
                hasPommel: command.HasPommel,
                includesSheath: command.IncludesSheath,
                sheathMaterial: command.SheathMaterial,
                isCoated: command.IsCoated,
                coatingType: command.CoatingType,
                discountPrice: command.DiscountPrice);

            if (!result.IsSuccess)
                throw new ValidationException(new Dictionary<string, string[]>
            {
                { "knife", [result.Error!] }
            });

            var knife = result.Value!;

            // 4. Створюємо Stock для ножа (початково 0)
            var stock = Stock.Create(knife.Id);

            // 5. Зберігаємо все в одній транзакції
            await _uow.BeginTransactionAsync(cancellationToken);
            try
            {
                await _uow.Knives.AddAsync(knife, cancellationToken);
                await _uow.Stock.AddAsync(stock, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                await _uow.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            return knife.Id;
        }
    }
}
