using FluentValidation;

namespace BladeVault.Application.Stocks.Commands.ChangeStockBalance
{
    public class ChangeStockBalanceCommandValidator : AbstractValidator<ChangeStockBalanceCommand>
    {
        public ChangeStockBalanceCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Товар обов'язковий");

            RuleFor(x => x.PerformedByUserId)
                .NotEmpty().WithMessage("Користувач обов'язковий");

            RuleFor(x => x.Delta)
                .NotEqual(0).WithMessage("Зміна кількості не може бути 0");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Причина зміни обов'язкова")
                .MaximumLength(300).WithMessage("Причина не може перевищувати 300 символів");

            RuleFor(x => x.DocumentReference)
                .MaximumLength(100).WithMessage("Посилання на документ не може перевищувати 100 символів");
        }
    }
}
