using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces;
using MediatR;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Users.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(
            IUnitOfWork uow,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResult> Handle(
            LoginUserCommand command,
            CancellationToken cancellationToken)
        {
            // 1. Шукаємо користувача по email
            var user = await _uow.Users.GetByEmailAsync(command.Email, cancellationToken)
                ?? throw new NotFoundException(nameof(User), command.Email);

            // 2. Перевіряємо пароль
            if (!_passwordHasher.Verify(command.Password, user.PasswordHash))
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { "credentials", ["Невірний email або пароль"] }
            });

            // 3. Перевіряємо чи активний акаунт
            if (!user.IsActive)
                throw new ApplicationValidationException(new Dictionary<string, string[]>
            {
                { "account", ["Акаунт деактивовано"] }
            });

            // 4. Генеруємо JWT токен
            var token = _jwtTokenGenerator.Generate(user);

            return new LoginResult(
                AccessToken: token,
                UserId: user.Id,
                FullName: $"{user.FirstName} {user.LastName}",
                Role: user.Role.ToString(),
                MustChangePassword: user.MustChangePassword);
        }
    }
}
