using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUnitOfWork _uow;

        public DeleteUserCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByIdAsync(command.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Domain.Entities.User), command.UserId);

            _uow.Users.Delete(user);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
