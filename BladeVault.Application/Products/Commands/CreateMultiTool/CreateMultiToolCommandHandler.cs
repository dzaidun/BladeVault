using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Interfaces;
using MediatR;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Products.Commands.CreateMultiTool
{
    public class CreateMultiToolCommandHandler : IRequestHandler<CreateMultiToolCommand, Guid>
    {
        private readonly IUnitOfWork _uow;

        public CreateMultiToolCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Handle(
            CreateMultiToolCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Перевіряємо чи існує категорія
            var category = await _uow.Categories.GetByIdAsync(command.CategoryId, cancellationToken)
                ?? throw new NotFoundException(nameof(Category), command.CategoryId);

            // 2. Перевіряємо унікальність SKU і Slug
            if (!await _uow.Products.IsSkuUniqueAsync(command.SKU, cancellationToken))
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { nameof(command.SKU), ["Товар з таким SKU вже існує"] }
            });

            if (!await _uow.Products.IsSlugUniqueAsync(command.Slug, cancellationToken))
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { nameof(command.Slug), ["Товар з таким Slug вже існує"] }
            });

            // 3. Створюємо мультитул через доменний метод
            var result = MultiTool.Create(
                name: command.Name,
                slug: command.Slug,
                sku: command.SKU,
                brand: command.Brand,
                model: command.Model,
                countryOfOrigin: command.CountryOfOrigin,
                categoryId: command.CategoryId,
                price: command.Price,
                handleMaterial: command.HandleMaterial,
                hasPliers: command.HasPliers,
                description: command.Description,
                weightGrams: command.WeightGrams,
                overallLengthMm: command.OverallLengthMm,
                closedLengthMm: command.ClosedLengthMm,
                replaceableWireCutters: command.ReplaceableWireCutters,
                hasLocking: command.HasLocking,
                isOneHandOpenable: command.IsOneHandOpenable,
                includesPouch: command.IncludesPouch,
                pouchMaterial: command.PouchMaterial,
                hasBitSet: command.HasBitSet,
                bitCount: command.BitCount,
                discountPrice: command.DiscountPrice);

            if (!result.IsSuccess)
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { "multiTool", [result.Error!] }
            });

            var multiTool = result.Value!;

            // 4. Додаємо інструменти
            foreach (var toolDto in command.IncludedTools)
            {
                var toolResult = multiTool.AddTool(toolDto.Type, toolDto.Description);
                if (!toolResult.IsSuccess)
                    throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { "tool", [toolResult.Error!] }
                });
            }

            // 5. Створюємо Stock
            var stock = Stock.Create(multiTool.Id);

            // 6. ��берігаємо в одній транзакції
            await _uow.BeginTransactionAsync(cancellationToken);
            try
            {
                await _uow.MultiTools.AddAsync(multiTool, cancellationToken);
                await _uow.Stock.AddAsync(stock, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                await _uow.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            return multiTool.Id;
        }
    }
}
