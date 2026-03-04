using MediatR;

namespace BladeVault.Application.Users.Commands.DeactivateUser
{
    public record DeactivateUserCommand(Guid UserId) : IRequest;
}
