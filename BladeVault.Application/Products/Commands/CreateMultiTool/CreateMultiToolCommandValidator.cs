using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Products.Commands.CreateMultiTool
{
    public class CreateMultiToolCommandValidator : AbstractValidator<CreateMultiToolCommand>
    {
        public CreateMultiToolCommandValidator()
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

            // Специфічні параметри
            RuleFor(x => x.HandleMaterial)
                .NotEmpty().WithMessage("Матеріал корпусу не може бути порожнім")
                .MaximumLength(100).WithMessage("Матеріал корпусу не може перевищувати 100 символів");

            RuleFor(x => x.BitCount)
                .GreaterThan(0)
                .When(x => x.HasBitSet)
                .WithMessage("Вкажіть кількість біт у наборі");

            RuleFor(x => x.PouchMaterial)
                .NotEmpty()
                .When(x => x.IncludesPouch)
                .WithMessage("Вкажіть матеріал чохла");

            // Список інструментів
            RuleFor(x => x.IncludedTools)
                .NotEmpty().WithMessage("Мультитул повинен мати хоча б один інструмент");

            RuleForEach(x => x.IncludedTools)
                .ChildRules(tool =>
                {
                    tool.RuleFor(x => x.Type)
                        .IsInEnum().WithMessage("Невідомий тип інструменту");
                });
        }
    }
}
