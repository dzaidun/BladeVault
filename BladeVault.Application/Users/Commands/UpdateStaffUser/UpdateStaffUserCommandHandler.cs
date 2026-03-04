using BladeVault.Application.Common.Exceptions;
using BladeVault.Domain.Enums;
using BladeVault.Domain.Interfaces;
using MediatR;

namespace BladeVault.Application.Users.Commands.UpdateStaffUser
{
    public class UpdateStaffUserCommandHandler : IRequestHandler<UpdateStaffUserCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateStaffUserCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(UpdateStaffUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _uow.Users.GetByIdAsync(command.StaffUserId, cancellationToken)
                ?? throw new NotFoundException(nameof(Domain.Entities.User), command.StaffUserId);

            if (!Enum.TryParse<UserRole>(command.Role, out var role))
                throw new ValidationException(new Dictionary<string, string[]>
                {
                    { "role", [$"Невалідна роль: {command.Role}"] }
                });

            user.Update(command.FirstName, command.LastName, command.PhoneNumber);

            _uow.Users.Update(user);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
