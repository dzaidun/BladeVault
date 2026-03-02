using FluentValidation;

namespace BladeVault.Application.CallCenter.Commands.CreateCallLog
{
    public class CreateCallLogCommandValidator : AbstractValidator<CreateCallLogCommand>
    {
        public CreateCallLogCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Клієнт обов'язковий");

            RuleFor(x => x.PerformedByUserId)
                .NotEmpty().WithMessage("Оператор обов'язковий");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Невідомий статус дзвінка");

            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Коментар не може перевищувати 500 символів");
        }
    }
}
