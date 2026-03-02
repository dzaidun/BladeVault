using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Entities;
using BladeVault.Domain.Interfaces;
using MediatR;
using ApplicationValidationException = BladeVault.Application.Common.Exceptions.ValidationException;

namespace BladeVault.Application.Users.Commands.ChangeTemporaryPassword
{
    public class ChangeTemporaryPasswordCommandHandler : IRequestHandler<ChangeTemporaryPasswordCommand>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        public ChangeTemporaryPasswordCommandHandler(IUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ChangeTemporaryPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByIdAsync(command.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(User), command.UserId);

            if (!user.MustChangePassword)
                throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { "password", ["Зміна тимчасового пароля не потрібна"] }
                });

            if (!_passwordHasher.Verify(command.TemporaryPassword, user.PasswordHash))
                throw new ApplicationValidationException(new Dictionary<string, string[]>
                {
                    { "password", ["Невірний тимчасовий пароль"] }
                });

            user.ChangePassword(_passwordHasher.Hash(command.NewPassword));

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
