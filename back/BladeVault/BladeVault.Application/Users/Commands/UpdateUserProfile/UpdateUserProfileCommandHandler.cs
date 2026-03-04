using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Users.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateUserProfileCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByIdAsync(command.UserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Domain.Entities.User), command.UserId);

            user.Update(command.FirstName, command.LastName, command.PhoneNumber);

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
