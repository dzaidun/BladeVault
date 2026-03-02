using FluentValidation;

namespace BladeVault.Application.Users.Commands.ChangeTemporaryPassword
{
    public class ChangeTemporaryPasswordCommandValidator : AbstractValidator<ChangeTemporaryPasswordCommand>
    {
        public ChangeTemporaryPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Користувач обов'язковий");

            RuleFor(x => x.TemporaryPassword)
                .NotEmpty().WithMessage("Тимчасовий пароль обов'язковий");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Новий пароль не може бути порожнім")
                .MinimumLength(8).WithMessage("Пароль має бути не менше 8 символів")
                .Matches("[A-Z]").WithMessage("Пароль має містити хоча б одну велику літеру")
                .Matches("[0-9]").WithMessage("Пароль має містити хоча б одну цифру");

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword).WithMessage("Паролі не співпадають");
        }
    }
}
