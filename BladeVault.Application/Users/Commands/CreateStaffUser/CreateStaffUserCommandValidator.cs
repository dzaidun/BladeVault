using BladeVault.Domain.Enums;
using FluentValidation;

namespace BladeVault.Application.Users.Commands.CreateStaffUser
{
    public class CreateStaffUserCommandValidator : AbstractValidator<CreateStaffUserCommand>
    {
        public CreateStaffUserCommandValidator()
        {
            RuleFor(x => x.CreatedByUserId)
                .NotEmpty().WithMessage("Відсутній користувач-ініціатор");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ім'я не може бути порожнім")
                .MaximumLength(100).WithMessage("Ім'я не може перевищувати 100 символів");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Прізвище не може бути порожнім")
                .MaximumLength(100).WithMessage("Прізвище не може перевищувати 100 символів");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email не може бути порожнім")
                .EmailAddress().WithMessage("Невірний формат Email");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Номер телефону не може бути порожнім")
                .Matches(@"^\+380\d{9}$").WithMessage("Невірний формат телефону. Приклад: +380991234567");

            RuleFor(x => x.Role)
                .Must(role => role != UserRole.Customer)
                .WithMessage("Для працівника потрібно вказати службову роль");
        }
    }
}
