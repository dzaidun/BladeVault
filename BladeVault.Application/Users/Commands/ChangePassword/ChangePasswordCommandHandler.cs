using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;

        public ChangePasswordCommandHandler(IUnitOfWork uow, IPasswordHasher passwordHasher)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByIdAsync(command.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Domain.Entities.User), command.UserId);

            if (!_passwordHasher.Verify(command.CurrentPassword, user.PasswordHash))
                throw new ValidationException(new Dictionary<string, string[]>
                {
                    { "currentPassword", ["Поточний пароль невірний"] }
                });

            var newPasswordHash = _passwordHasher.Hash(command.NewPassword);
            user.ChangePassword(newPasswordHash);

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
