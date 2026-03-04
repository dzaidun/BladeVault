using BladeVault.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Користувач обов'язковий");

            RuleFor(x => x.DeliveryMethod)
                .IsInEnum().WithMessage("Невідомий спосіб доставки");

            // Якщо доставка — потрібна адреса
            RuleFor(x => x.AddressId)
                .NotNull()
                .When(x => x.DeliveryMethod == DeliveryMethod.Courier)
                .WithMessage("Для кур'єрської доставки потрібна адреса");

            // Якщо Нова Пошта — потрібне відділення
            RuleFor(x => x.NovaPostWarehouse)
                .NotEmpty()
                .When(x => x.DeliveryMethod == DeliveryMethod.NovaPost)
                .WithMessage("Вкажіть відділення Нової Пошти");

            // Товари
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Замовлення не може бути порожнім");

            RuleForEach(x => x.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(x => x.ProductId)
                        .NotEmpty().WithMessage("Id товару не може бути порожнім");

                    item.RuleFor(x => x.Quantity)
                        .GreaterThan(0).WithMessage("Кількість має бути більше 0");
                });
        }
    }
}
