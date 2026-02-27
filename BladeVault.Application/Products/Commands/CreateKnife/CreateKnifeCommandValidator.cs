using FluentValidation;
using BladeVault.Domain.Enums.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Commands.CreateKnife
{
    public class CreateKnifeCommandValidator : AbstractValidator<CreateKnifeCommand>
    {
        public CreateKnifeCommandValidator()
        {
            // Базові поля
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва не може бути порожньою")
                .MaximumLength(200).WithMessage("Назва не може перевищувати 200 символів");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug не може бути порожнім")
                .MaximumLength(200).WithMessage("Slug не може перевищувати 200 символів")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug може містити лише малі літери, цифри та дефіс");

            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU не може бути порожнім")
                .MaximumLength(50).WithMessage("SKU не може перевищувати 50 символів");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Бренд не може бути порожнім")
                .MaximumLength(100).WithMessage("Бренд не може перевищувати 100 символів");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Модель не може бути порожньою")
                .MaximumLength(100).WithMessage("Модель не може перевищувати 100 символів");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Категорія обов'язкова");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ціна має бути більше 0");

            RuleFor(x => x.DiscountPrice)
                .LessThan(x => x.Price)
                .When(x => x.DiscountPrice.HasValue)
                .WithMessage("Ціна зі знижкою має бути менша за звичайну ціну");

            // Клинок
            RuleFor(x => x.SteelType)
                .NotEmpty().WithMessage("Тип сталі не може бути порожнім")
                .MaximumLength(50).WithMessage("Тип сталі не може перевищувати 50 символів");

            RuleFor(x => x.BladeLengthMm)
                .GreaterThan(0).WithMessage("Довжина клинка має бути більше 0")
                .LessThanOrEqualTo(500).WithMessage("Довжина клинка не може перевищувати 500мм");

            RuleFor(x => x.BladeThicknessMm)
                .GreaterThan(0).WithMessage("Товщина клинка має бути більше 0")
                .LessThanOrEqualTo(20).WithMessage("Товщина клинка не може перевищувати 20мм");

            RuleFor(x => x.HandleMaterial)
                .NotEmpty().WithMessage("Матеріал руків'я не може бути порожнім")
                .MaximumLength(100).WithMessage("Матеріал руків'я не може перевищувати 100 символів");

            // Складний ніж — обов'язкові поля
            RuleFor(x => x.LockType)
                .NotNull()
                .When(x => x.KnifeType == KnifeType.Folding)
                .WithMessage("Складний ніж повинен мати тип замка");

            RuleFor(x => x.OpeningMechanism)
                .NotNull()
                .When(x => x.KnifeType == KnifeType.Folding)
                .WithMessage("Складний ніж повинен мати механізм відкриття");

            RuleFor(x => x.CoatingType)
                .NotEmpty()
                .When(x => x.IsCoated)
                .WithMessage("Вкажіть тип покриття клинка");

            RuleFor(x => x.SheathMaterial)
                .NotEmpty()
                .When(x => x.IncludesSheath)
                .WithMessage("Вкажіть матеріал піхов");
        }
    }
}
