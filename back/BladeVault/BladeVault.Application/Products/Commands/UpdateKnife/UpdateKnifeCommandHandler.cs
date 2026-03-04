using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities.Products;
using BladeVault.Domain.Interfaces;
using MediatR;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Products.Commands.UpdateKnife
{
    public class UpdateKnifeCommandHandler : IRequestHandler<UpdateKnifeCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateKnifeCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(
            UpdateKnifeCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Знаходимо ніж
            var knife = await _uow.Knives.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Knife), command.Id);

            // 2. Перевіряємо унікальність Slug
            // (якщо slug змінився — перевіряємо що новий не зайнятий)
            if (knife.Slug != command.Slug &&
                !await _uow.Products.IsSlugUniqueAsync(command.Slug, cancellationToken))
            {
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { nameof(command.Slug), ["Товар з таким Slug вже існує"] }
            });
            }

            // 3. Оновлюємо базові поля через метод з Domain
            knife.UpdateBaseInfo(
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
            knife.UpdatePrice(command.Price, command.DiscountPrice);

            // 5. Оновлюємо специфічні поля ножа
            var result = knife.UpdateKnifeDetails(
                steelType: command.SteelType,
                bladeLengthMm: command.BladeLengthMm,
                bladeThicknessMm: command.BladeThicknessMm,
                bladeShape: command.BladeShape,
                edgeType: command.EdgeType,
                handleMaterial: command.HandleMaterial,
                lockType: command.LockType,
                openingMechanism: command.OpeningMechanism,
                hasClip: command.HasClip,
                hasGuard: command.HasGuard,
                hasPommel: command.HasPommel,
                includesSheath: command.IncludesSheath,
                sheathMaterial: command.SheathMaterial,
                isCoated: command.IsCoated,
                coatingType: command.CoatingType);

            if (!result.IsSuccess)
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { "knife", [result.Error!] }
            });

            // 6. Зберігаємо
            _uow.Knives.Update(knife);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
