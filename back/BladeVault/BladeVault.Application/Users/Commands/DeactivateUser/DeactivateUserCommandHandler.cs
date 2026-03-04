using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
    {
        private readonly IUnitOfWork _uow;

        public DeactivateUserCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(DeactivateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByIdAsync(command.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Domain.Entities.User), command.UserId);

            user.Deactivate();

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
