using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces;
using MediatR;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(
            RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Перевіряємо унікальність Email і телефону
            if (!await _uow.Users.IsEmailUniqueAsync(command.Email, cancellationToken))
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { nameof(command.Email), ["Користувач з таким Email вже існує"] }
            });

            if (!await _uow.Users.IsPhoneUniqueAsync(command.PhoneNumber, cancellationToken))
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { nameof(command.PhoneNumber), ["Користувач з таким номером телефону вже існує"] }
            });

            // 2. Хешуємо пароль
            var passwordHash = _passwordHasher.Hash(command.Password);

            // 3. Створюємо користувача
            var user = User.Create(
                firstName: command.FirstName,
                lastName: command.LastName,
                email: command.Email,
                phoneNumber: command.PhoneNumber,
                passwordHash: passwordHash);

            // 4. Зберігаємо
            await _uow.Users.AddAsync(user, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
