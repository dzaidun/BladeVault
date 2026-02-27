using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Interfaces;
using MediatR;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Products.Commands.UpdateMultiTool
{
    public class UpdateMultiToolCommandHandler : IRequestHandler<UpdateMultiToolCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateMultiToolCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(
            UpdateMultiToolCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Знаходимо мультитул разом з інструментами
            var multiTool = await _uow.MultiTools.GetWithToolsAsync(command.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(MultiTool), command.Id);

            // 2. Перевіряємо унікальність Slug якщо змінився
            if (multiTool.Slug != command.Slug &&
                !await _uow.Products.IsSlugUniqueAsync(command.Slug, cancellationToken))
            {
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { nameof(command.Slug), ["Товар з таким Slug вже існує"] }
            });
            }

            // 3. Оновлюємо базові поля
            multiTool.UpdateBaseInfo(
                name: command.Name,
                slug: command.Slug,
                brand: command.Brand,
                model: command.Model,
                countryOfOrigin: command.CountryOfOrigin,
                description: command.Description,
                weightGrams: command.WeightGrams,
                overallLengthMm: command.OverallLengthMm,
                closedLengthMm: command.ClosedLengthMm);

            // 4. Оновлюємо ціну
            multiTool.UpdatePrice(command.Price, command.DiscountPrice);

            // 5. Оновлюємо специфічні поля
            var result = multiTool.UpdateMultiToolDetails(
                handleMaterial: command.HandleMaterial,
                hasPliers: command.HasPliers,
                replaceableWireCutters: command.ReplaceableWireCutters,
                hasLocking: command.HasLocking,
                isOneHandOpenable: command.IsOneHandOpenable,
                includesPouch: command.IncludesPouch,
                pouchMaterial: command.PouchMaterial,
                hasBitSet: command.HasBitSet,
                bitCount: command.BitCount);

            if (!result.IsSuccess)
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { "multiTool", [result.Error!] }
            });

            // 6. Замінюємо список інструментів повністю
            // Видаляємо всі старі
            foreach (var tool in multiTool.IncludedTools.ToList())
                multiTool.RemoveTool(tool.Id);

            // Додаємо нові
            foreach (var toolDto in command.IncludedTools)
            {
                var toolResult = multiTool.AddTool(toolDto.Type, toolDto.Description);
                if (!toolResult.IsSuccess)
                    throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { "tool", [toolResult.Error!] }
                });
            }

            // 7. Зберігаємо
            _uow.MultiTools.Update(multiTool);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
