using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ім'я не може бути порожнім")
                .MaximumLength(100).WithMessage("Ім'я не може перевищувати 100 символів");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("��різвище не може бути порожнім")
                .MaximumLength(100).WithMessage("Прізвище не може перевищувати 100 символів");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email не може бути порожнім")
                .EmailAddress().WithMessage("Невірний формат Email");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Номер телефону не може бути порожнім")
                .Matches(@"^\+380\d{9}$").WithMessage("Невірний формат телефону. Приклад: +380991234567");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль не може бути порожнім")
                .MinimumLength(8).WithMessage("Пароль має бути не менше 8 символів")
                .Matches("[A-Z]").WithMessage("Пароль має містити хоча б одну велику літеру")
                .Matches("[0-9]").WithMessage("Пароль має містити хоча б одну цифру");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Паролі не співпадають");
        }
    }
}
